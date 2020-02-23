/* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  */
/* UTM / WGS-84 Conversion Functions                                  (c) Chris Veness 2014-2017  */
/*                                                                                   MIT Licence  */
/* www.movable-type.co.uk/scripts/latlong-utm-mgrs.html                                           */
/* www.movable-type.co.uk/scripts/geodesy/docs/module-utm.html                                    */
/* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  */

'use strict';
if (typeof module != 'undefined' && module.exports) var LatLon = require('./latlon-ellipsoidal.js'); // ≡ import LatLon from 'latlon-ellipsoidal.js'
/* eslint indent:warn */


/**
 * Convert between Universal Transverse Mercator coordinates and WGS 84 latitude/longitude points.
 *
 * Method based on Karney 2011 ‘Transverse Mercator with an accuracy of a few nanometers’,
 * building on Krüger 1912 ‘Konforme Abbildung des Erdellipsoids in der Ebene’.
 *
 * @module   utm
 * @requires latlon-ellipsoidal
 */


/**
 * Creates a Utm coordinate object.
 *
 * @constructor
 * @param  {number} zone - UTM 6° longitudinal zone (1..60 covering 180°W..180°E).
 * @param  {string} hemisphere - N for northern hemisphere, S for southern hemisphere.
 * @param  {number} easting - Easting in metres from false easting (-500km from central meridian).
 * @param  {number} northing - Northing in metres from equator (N) or from false northing -10,000km (S).
 * @param  {LatLon.datum} [datum=WGS84] - Datum UTM coordinate is based on.
 * @param  {number} [convergence] - Meridian convergence (bearing of grid north clockwise from true
 *                  north), in degrees
 * @param  {number} [scale] - Grid scale factor
 * @throws {Error}  Invalid UTM coordinate
 *
 * @example
 *   var utmCoord = new Utm(31, 'N', 448251, 5411932);
 */
function Utm(zone, hemisphere, easting, northing, datum, convergence, scale) {
    if (!(this instanceof Utm)) { // allow instantiation without 'new'
        return new Utm(zone, hemisphere, easting, northing, datum, convergence, scale);
    }

    if (datum === undefined) datum = LatLon.datum.WGS84; // default if not supplied
    if (convergence === undefined) convergence = null;   // default if not supplied
    if (scale === undefined) scale = null;               // default if not supplied

    if (!(1 <= zone && zone <= 60)) throw new Error('Invalid UTM zone ' + zone);
    if (!hemisphere.match(/[NS]/i)) throw new Error('Invalid UTM hemisphere ' + hemisphere);
    // range-check easting/northing (with 40km overlap between zones) - is this worthwhile?
    //if (!(120e3<=easting && easting<=880e3)) throw new Error('Invalid UTM easting '+ easting);
    //if (!(0<=northing && northing<=10000e3)) throw new Error('Invalid UTM northing '+ northing);

    this.zone = Number(zone);
    this.hemisphere = hemisphere.toUpperCase();
    this.easting = Number(easting);
    this.northing = Number(northing);
    this.datum = datum;
    this.convergence = convergence === null ? null : Number(convergence);
    this.scale = scale === null ? null : Number(scale);
}


/**
 * Converts latitude/longitude to UTM coordinate.
 *
 * Implements Karney’s method, using Krüger series to order n^6, giving results accurate to 5nm for
 * distances up to 3900km from the central meridian.
 *
 * @returns {Utm}   UTM coordinate.
 * @throws  {Error} If point not valid, if point outside latitude range.
 *
 * @example
 *   var latlong = new LatLon(48.8582, 2.2945);
 *   var utmCoord = latlong.toUtm(); // utmCoord.toString(): '31 N 448252 5411933'
 */
LatLon.prototype.toUtm = function () {
    if (isNaN(this.lat) || isNaN(this.lon)) throw new Error('Invalid point');
    if (!(-80 <= this.lat && this.lat <= 84)) throw new Error('Outside UTM limits');

    var falseEasting = 500e3, falseNorthing = 10000e3;

    var zone = Math.floor((this.lon + 180) / 6) + 1; // longitudinal zone
    var λ0 = ((zone - 1) * 6 - 180 + 3).toRadians(); // longitude of central meridian

    // ---- handle Norway/Svalbard exceptions
    // grid zones are 8° tall; 0°N is offset 10 into latitude bands array
    var mgrsLatBands = 'CDEFGHJKLMNPQRSTUVWXX'; // X is repeated for 80-84°N
    var latBand = mgrsLatBands.charAt(Math.floor(this.lat / 8 + 10));
    // adjust zone & central meridian for Norway
    if (zone == 31 && latBand == 'V' && this.lon >= 3) { zone++; λ0 += (6).toRadians(); }
    // adjust zone & central meridian for Svalbard
    if (zone == 32 && latBand == 'X' && this.lon < 9) { zone--; λ0 -= (6).toRadians(); }
    if (zone == 32 && latBand == 'X' && this.lon >= 9) { zone++; λ0 += (6).toRadians(); }
    if (zone == 34 && latBand == 'X' && this.lon < 21) { zone--; λ0 -= (6).toRadians(); }
    if (zone == 34 && latBand == 'X' && this.lon >= 21) { zone++; λ0 += (6).toRadians(); }
    if (zone == 36 && latBand == 'X' && this.lon < 33) { zone--; λ0 -= (6).toRadians(); }
    if (zone == 36 && latBand == 'X' && this.lon >= 33) { zone++; λ0 += (6).toRadians(); }

    var φ = this.lat.toRadians();      // latitude ± from equator
    var λ = this.lon.toRadians() - λ0; // longitude ± from central meridian

    var a = this.datum.ellipsoid.a, f = this.datum.ellipsoid.f;
    // WGS 84: a = 6378137, b = 6356752.314245, f = 1/298.257223563;

    var k0 = 0.9996; // UTM scale on the central meridian

    // ---- easting, northing: Karney 2011 Eq 7-14, 29, 35:

    var e = Math.sqrt(f * (2 - f)); // eccentricity
    var n = f / (2 - f);        // 3rd flattening
    var n2 = n * n, n3 = n * n2, n4 = n * n3, n5 = n * n4, n6 = n * n5; // TODO: compare Horner-form accuracy?

    var cosλ = Math.cos(λ), sinλ = Math.sin(λ), tanλ = Math.tan(λ);

    var τ = Math.tan(φ); // τ ≡ tanφ, τʹ ≡ tanφʹ; prime (ʹ) indicates angles on the conformal sphere
    var σ = Math.sinh(e * Math.atanh(e * τ / Math.sqrt(1 + τ * τ)));

    var τʹ = τ * Math.sqrt(1 + σ * σ) - σ * Math.sqrt(1 + τ * τ);

    var ξʹ = Math.atan2(τʹ, cosλ);
    var ηʹ = Math.asinh(sinλ / Math.sqrt(τʹ * τʹ + cosλ * cosλ));

    var A = a / (1 + n) * (1 + 1 / 4 * n2 + 1 / 64 * n4 + 1 / 256 * n6); // 2πA is the circumference of a meridian

    var α = [null, // note α is one-based array (6th order Krüger expressions)
        1 / 2 * n - 2 / 3 * n2 + 5 / 16 * n3 + 41 / 180 * n4 - 127 / 288 * n5 + 7891 / 37800 * n6,
              13 / 48 * n2 - 3 / 5 * n3 + 557 / 1440 * n4 + 281 / 630 * n5 - 1983433 / 1935360 * n6,
                       61 / 240 * n3 - 103 / 140 * n4 + 15061 / 26880 * n5 + 167603 / 181440 * n6,
                               49561 / 161280 * n4 - 179 / 168 * n5 + 6601661 / 7257600 * n6,
                                                 34729 / 80640 * n5 - 3418889 / 1995840 * n6,
                                                              212378941 / 319334400 * n6];

    var ξ = ξʹ;
    for (var j = 1; j <= 6; j++) ξ += α[j] * Math.sin(2 * j * ξʹ) * Math.cosh(2 * j * ηʹ);

    var η = ηʹ;
    for (var j = 1; j <= 6; j++) η += α[j] * Math.cos(2 * j * ξʹ) * Math.sinh(2 * j * ηʹ);

    var x = k0 * A * η;
    var y = k0 * A * ξ;

    // ---- convergence: Karney 2011 Eq 23, 24

    var pʹ = 1;
    for (var j = 1; j <= 6; j++) pʹ += 2 * j * α[j] * Math.cos(2 * j * ξʹ) * Math.cosh(2 * j * ηʹ);
    var qʹ = 0;
    for (var j = 1; j <= 6; j++) qʹ += 2 * j * α[j] * Math.sin(2 * j * ξʹ) * Math.sinh(2 * j * ηʹ);

    var γʹ = Math.atan(τʹ / Math.sqrt(1 + τʹ * τʹ) * tanλ);
    var γʺ = Math.atan2(qʹ, pʹ);

    var γ = γʹ + γʺ;

    // ---- scale: Karney 2011 Eq 25

    var sinφ = Math.sin(φ);
    var kʹ = Math.sqrt(1 - e * e * sinφ * sinφ) * Math.sqrt(1 + τ * τ) / Math.sqrt(τʹ * τʹ + cosλ * cosλ);
    var kʺ = A / a * Math.sqrt(pʹ * pʹ + qʹ * qʹ);

    var k = k0 * kʹ * kʺ;

    // ------------

    // shift x/y to false origins
    x = x + falseEasting;             // make x relative to false easting
    if (y < 0) y = y + falseNorthing; // make y in southern hemisphere relative to false northing

    // round to reasonable precision
    x = Number(x.toFixed(6)); // nm precision
    y = Number(y.toFixed(6)); // nm precision
    var convergence = Number(γ.toDegrees().toFixed(9));
    var scale = Number(k.toFixed(12));

    var h = this.lat >= 0 ? 'N' : 'S'; // hemisphere

    return new Utm(zone, h, x, y, this.datum, convergence, scale);
};


/**
 * Converts UTM zone/easting/northing coordinate to latitude/longitude
 *
 * @param   {Utm}    utmCoord - UTM coordinate to be converted to latitude/longitude.
 * @returns {LatLon} Latitude/longitude of supplied grid reference.
 *
 * @example
 *   var grid = new Utm(31, 'N', 448251.795, 5411932.678);
 *   var latlong = grid.toLatLonE(); // latlong.toString(): 48°51′29.52″N, 002°17′40.20″E
 */
Utm.prototype.toLatLonE = function () {
    var z = this.zone;
    var h = this.hemisphere;
    var x = this.easting;
    var y = this.northing;

    if (isNaN(z) || isNaN(x) || isNaN(y)) throw new Error('Invalid coordinate');

    var falseEasting = 500e3, falseNorthing = 10000e3;

    var a = this.datum.ellipsoid.a, f = this.datum.ellipsoid.f;
    // WGS 84:  a = 6378137, b = 6356752.314245, f = 1/298.257223563;

    var k0 = 0.9996; // UTM scale on the central meridian

    x = x - falseEasting;               // make x ± relative to central meridian
    y = h == 'S' ? y - falseNorthing : y; // make y ± relative to equator

    // ---- from Karney 2011 Eq 15-22, 36:

    var e = Math.sqrt(f * (2 - f)); // eccentricity
    var n = f / (2 - f);        // 3rd flattening
    var n2 = n * n, n3 = n * n2, n4 = n * n3, n5 = n * n4, n6 = n * n5;

    var A = a / (1 + n) * (1 + 1 / 4 * n2 + 1 / 64 * n4 + 1 / 256 * n6); // 2πA is the circumference of a meridian

    var η = x / (k0 * A);
    var ξ = y / (k0 * A);

    var β = [null, // note β is one-based array (6th order Krüger expressions)
        1 / 2 * n - 2 / 3 * n2 + 37 / 96 * n3 - 1 / 360 * n4 - 81 / 512 * n5 + 96199 / 604800 * n6,
               1 / 48 * n2 + 1 / 15 * n3 - 437 / 1440 * n4 + 46 / 105 * n5 - 1118711 / 3870720 * n6,
                        17 / 480 * n3 - 37 / 840 * n4 - 209 / 4480 * n5 + 5569 / 90720 * n6,
                                 4397 / 161280 * n4 - 11 / 504 * n5 - 830251 / 7257600 * n6,
                                               4583 / 161280 * n5 - 108847 / 3991680 * n6,
                                                             20648693 / 638668800 * n6];

    var ξʹ = ξ;
    for (var j = 1; j <= 6; j++) ξʹ -= β[j] * Math.sin(2 * j * ξ) * Math.cosh(2 * j * η);

    var ηʹ = η;
    for (var j = 1; j <= 6; j++) ηʹ -= β[j] * Math.cos(2 * j * ξ) * Math.sinh(2 * j * η);

    var sinhηʹ = Math.sinh(ηʹ);
    var sinξʹ = Math.sin(ξʹ), cosξʹ = Math.cos(ξʹ);

    var τʹ = sinξʹ / Math.sqrt(sinhηʹ * sinhηʹ + cosξʹ * cosξʹ);

    var τi = τʹ;
    do {
        var σi = Math.sinh(e * Math.atanh(e * τi / Math.sqrt(1 + τi * τi)));
        var τiʹ = τi * Math.sqrt(1 + σi * σi) - σi * Math.sqrt(1 + τi * τi);
        var δτi = (τʹ - τiʹ) / Math.sqrt(1 + τiʹ * τiʹ)
            * (1 + (1 - e * e) * τi * τi) / ((1 - e * e) * Math.sqrt(1 + τi * τi));
        τi += δτi;
    } while (Math.abs(δτi) > 1e-12); // using IEEE 754 δτi -> 0 after 2-3 iterations
    // note relatively large convergence test as δτi toggles on ±1.12e-16 for eg 31 N 400000 5000000
    var τ = τi;

    var φ = Math.atan(τ);

    var λ = Math.atan2(sinhηʹ, cosξʹ);

    // ---- convergence: Karney 2011 Eq 26, 27

    var p = 1;
    for (var j = 1; j <= 6; j++) p -= 2 * j * β[j] * Math.cos(2 * j * ξ) * Math.cosh(2 * j * η);
    var q = 0;
    for (var j = 1; j <= 6; j++) q += 2 * j * β[j] * Math.sin(2 * j * ξ) * Math.sinh(2 * j * η);

    var γʹ = Math.atan(Math.tan(ξʹ) * Math.tanh(ηʹ));
    var γʺ = Math.atan2(q, p);

    var γ = γʹ + γʺ;

    // ---- scale: Karney 2011 Eq 28

    var sinφ = Math.sin(φ);
    var kʹ = Math.sqrt(1 - e * e * sinφ * sinφ) * Math.sqrt(1 + τ * τ) * Math.sqrt(sinhηʹ * sinhηʹ + cosξʹ * cosξʹ);
    var kʺ = A / a / Math.sqrt(p * p + q * q);

    var k = k0 * kʹ * kʺ;

    // ------------

    var λ0 = ((z - 1) * 6 - 180 + 3).toRadians(); // longitude of central meridian
    λ += λ0; // move λ from zonal to global coordinates

    // round to reasonable precision
    var lat = Number(φ.toDegrees().toFixed(11)); // nm precision (1nm = 10^-11°)
    var lon = Number(λ.toDegrees().toFixed(11)); // (strictly lat rounding should be φ⋅cosφ!)
    var convergence = Number(γ.toDegrees().toFixed(9));
    var scale = Number(k.toFixed(12));

    var latLong = new LatLon(lat, lon, this.datum);
    // ... and add the convergence and scale into the LatLon object ... wonderful JavaScript!
    latLong.convergence = convergence;
    latLong.scale = scale;

    return latLong;
};


/**
 * Parses string representation of UTM coordinate.
 *
 * A UTM coordinate comprises (space-separated)
 *  - zone
 *  - hemisphere
 *  - easting
 *  - northing.
 *
 * @param   {string} utmCoord - UTM coordinate (WGS 84).
 * @param   {Datum}  [datum=WGS84] - Datum coordinate is defined in (default WGS 84).
 * @returns {Utm}
 * @throws  {Error}  Invalid UTM coordinate.
 *
 * @example
 *   var utmCoord = Utm.parse('31 N 448251 5411932');
 *   // utmCoord: {zone: 31, hemisphere: 'N', easting: 448251, northing: 5411932 }
 */
Utm.parse = function (utmCoord, datum) {
    if (datum === undefined) datum = LatLon.datum.WGS84; // default if not supplied

    // match separate elements (separated by whitespace)
    utmCoord = utmCoord.trim().match(/\S+/g);

    if (utmCoord == null || utmCoord.length != 4) throw new Error('Invalid UTM coordinate ‘' + utmCoord + '’');

    var zone = utmCoord[0], hemisphere = utmCoord[1], easting = utmCoord[2], northing = utmCoord[3];

    return new Utm(zone, hemisphere, easting, northing, datum);
};


/**
 * Returns a string representation of a UTM coordinate.
 *
 * To distinguish from MGRS grid zone designators, a space is left between the zone and the
 * hemisphere.
 *
 * Note that UTM coordinates get rounded, not truncated (unlike MGRS grid references).
 *
 * @param   {number} [digits=0] - Number of digits to appear after the decimal point (3 ≡ mm).
 * @returns {string} A string representation of the coordinate.
 *
 * @example
 *   var utm = Utm.parse('31 N 448251 5411932').toString(4);  // 31 N 448251.0000 5411932.0000
 */
Utm.prototype.toString = function (digits) {
    digits = Number(digits || 0); // default 0 if not supplied

    var z = this.zone < 10 ? '0' + this.zone : this.zone; // leading zero
    var h = this.hemisphere;
    var e = this.easting;
    var n = this.northing;
    if (isNaN(z) || !h.match(/[NS]/) || isNaN(e) || isNaN(n)) return '';

    return z + ' ' + h + ' ' + e.toFixed(digits) + ' ' + n.toFixed(digits);
};


/* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  */

/** Polyfill Math.sinh for old browsers / IE */
if (Math.sinh === undefined) {
    Math.sinh = function (x) {
        return (Math.exp(x) - Math.exp(-x)) / 2;
    };
}

/** Polyfill Math.cosh for old browsers / IE */
if (Math.cosh === undefined) {
    Math.cosh = function (x) {
        return (Math.exp(x) + Math.exp(-x)) / 2;
    };
}

/** Polyfill Math.tanh for old browsers / IE */
if (Math.tanh === undefined) {
    Math.tanh = function (x) {
        return (Math.exp(x) - Math.exp(-x)) / (Math.exp(x) + Math.exp(-x));
    };
}

/** Polyfill Math.asinh for old browsers / IE */
if (Math.asinh === undefined) {
    Math.asinh = function (x) {
        return Math.log(x + Math.sqrt(1 + x * x));
    };
}

/** Polyfill Math.atanh for old browsers / IE */
if (Math.atanh === undefined) {
    Math.atanh = function (x) {
        return Math.log((1 + x) / (1 - x)) / 2;
    };
}

/* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  */
if (typeof module != 'undefined' && module.exports) module.exports = Utm; // ≡ export default Utm
/* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  */
/*  MGRS / UTM Conversion Functions                                   (c) Chris Veness 2014-2016  */
/*                                                                                   MIT Licence  */
/* www.movable-type.co.uk/scripts/latlong-utm-mgrs.html                                           */
/* www.movable-type.co.uk/scripts/geodesy/docs/module-mgrs.html                                   */
/* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  */

'use strict';
if (typeof module != 'undefined' && module.exports) var Utm = require('./utm.js');                   // ≡ import Utm from 'utm.js'
if (typeof module != 'undefined' && module.exports) var LatLon = require('./latlon-ellipsoidal.js'); // ≡ import LatLon from 'latlon-ellipsoidal.js'


/**
 * Convert between Universal Transverse Mercator (UTM) coordinates and Military Grid Reference
 * System (MGRS/NATO) grid references.
 *
 * @module   mgrs
 * @requires utm
 * @requires latlon-ellipsoidal
 */

/* qv www.fgdc.gov/standards/projects/FGDC-standards-projects/usng/fgdc_std_011_2001_usng.pdf p10 */


/*
 * Latitude bands C..X 8° each, covering 80°S to 84°N
 */
Mgrs.latBands = 'CDEFGHJKLMNPQRSTUVWXX'; // X is repeated for 80-84°N


/*
 * 100km grid square column (‘e’) letters repeat every third zone
 */
Mgrs.e100kLetters = ['ABCDEFGH', 'JKLMNPQR', 'STUVWXYZ'];


/*
 * 100km grid square row (‘n’) letters repeat every other zone
 */
Mgrs.n100kLetters = ['ABCDEFGHJKLMNPQRSTUV', 'FGHJKLMNPQRSTUVABCDE'];


/**
 * Creates an Mgrs grid reference object.
 *
 * @constructor
 * @param  {number} zone - 6° longitudinal zone (1..60 covering 180°W..180°E).
 * @param  {string} band - 8° latitudinal band (C..X covering 80°S..84°N).
 * @param  {string} e100k - First letter (E) of 100km grid square.
 * @param  {string} n100k - Second letter (N) of 100km grid square.
 * @param  {number} easting - Easting in metres within 100km grid square.
 * @param  {number} northing - Northing in metres within 100km grid square.
 * @param  {LatLon.datum} [datum=WGS84] - Datum UTM coordinate is based on.
 * @throws {Error}  Invalid MGRS grid reference.
 *
 * @example
 *   var mgrsRef = new Mgrs(31, 'U', 'D', 'Q', 48251, 11932); // 31U DQ 48251 11932
 */
function Mgrs(zone, band, e100k, n100k, easting, northing, datum) {
    // allow instantiation without 'new'
    if (!(this instanceof Mgrs)) return new Mgrs(zone, band, e100k, n100k, easting, northing, datum);

    if (datum === undefined) datum = LatLon.datum.WGS84; // default if not supplied

    if (!(1 <= zone && zone <= 60)) throw new Error('Invalid MGRS grid reference (zone ‘' + zone + '’)');
    if (band.length != 1) throw new Error('Invalid MGRS grid reference (band ‘' + band + '’)');
    if (Mgrs.latBands.indexOf(band) == -1) throw new Error('Invalid MGRS grid reference (band ‘' + band + '’)');
    if (e100k.length != 1) throw new Error('Invalid MGRS grid reference (e100k ‘' + e100k + '’)');
    if (n100k.length != 1) throw new Error('Invalid MGRS grid reference (n100k ‘' + n100k + '’)');

    this.zone = Number(zone);
    this.band = band;
    this.e100k = e100k;
    this.n100k = n100k;
    this.easting = Number(easting);
    this.northing = Number(northing);
    this.datum = datum;
}


/**
 * Converts UTM coordinate to MGRS reference.
 *
 * @returns {Mgrs}
 * @throws  {Error} Invalid UTM coordinate.
 *
 * @example
 *   var utmCoord = new Utm(31, 'N', 448251, 5411932);
 *   var mgrsRef = utmCoord.toMgrs(); // 31U DQ 48251 11932
 */
Utm.prototype.toMgrs = function () {
    if (isNaN(this.zone + this.easting + this.northing)) throw new Error('Invalid UTM coordinate ‘' + this.toString() + '’');

    // MGRS zone is same as UTM zone
    var zone = this.zone;

    // convert UTM to lat/long to get latitude to determine band
    var latlong = this.toLatLonE();
    // grid zones are 8° tall, 0°N is 10th band
    var band = Mgrs.latBands.charAt(Math.floor(latlong.lat / 8 + 10)); // latitude band

    // columns in zone 1 are A-H, zone 2 J-R, zone 3 S-Z, then repeating every 3rd zone
    var col = Math.floor(this.easting / 100e3);
    var e100k = Mgrs.e100kLetters[(zone - 1) % 3].charAt(col - 1); // col-1 since 1*100e3 -> A (index 0), 2*100e3 -> B (index 1), etc.

    // rows in even zones are A-V, in odd zones are F-E
    var row = Math.floor(this.northing / 100e3) % 20;
    var n100k = Mgrs.n100kLetters[(zone - 1) % 2].charAt(row);

    // truncate easting/northing to within 100km grid square
    var easting = this.easting % 100e3;
    var northing = this.northing % 100e3;

    // round to nm precision
    easting = Number(easting.toFixed(6));
    northing = Number(northing.toFixed(6));

    return new Mgrs(zone, band, e100k, n100k, easting, northing);
};


/**
 * Converts MGRS grid reference to UTM coordinate.
 *
 * @returns {Utm}
 *
 * @example
 *   var utmCoord = Mgrs.parse('31U DQ 448251 11932').toUtm(); // 31 N 448251 5411932
 */
Mgrs.prototype.toUtm = function () {
    var zone = this.zone;
    var band = this.band;
    var e100k = this.e100k;
    var n100k = this.n100k;
    var easting = this.easting;
    var northing = this.northing;

    var hemisphere = band >= 'N' ? 'N' : 'S';

    // get easting specified by e100k
    var col = Mgrs.e100kLetters[(zone - 1) % 3].indexOf(e100k) + 1; // index+1 since A (index 0) -> 1*100e3, B (index 1) -> 2*100e3, etc.
    var e100kNum = col * 100e3; // e100k in metres

    // get northing specified by n100k
    var row = Mgrs.n100kLetters[(zone - 1) % 2].indexOf(n100k);
    var n100kNum = row * 100e3; // n100k in metres

    // get latitude of (bottom of) band
    var latBand = (Mgrs.latBands.indexOf(band) - 10) * 8;

    // northing of bottom of band, extended to include entirety of bottommost 100km square
    // (100km square boundaries are aligned with 100km UTM northing intervals)
    var nBand = Math.floor(new LatLon(latBand, 0).toUtm().northing / 100e3) * 100e3;
    // 100km grid square row letters repeat every 2,000km north; add enough 2,000km blocks to get
    // into required band
    var n2M = 0; // northing of 2,000km block
    while (n2M + n100kNum + northing < nBand) n2M += 2000e3;

    return new Utm(zone, hemisphere, e100kNum + easting, n2M + n100kNum + northing, this.datum);
};


/**
 * Parses string representation of MGRS grid reference.
 *
 * An MGRS grid reference comprises (space-separated)
 *  - grid zone designator (GZD)
 *  - 100km grid square letter-pair
 *  - easting
 *  - northing.
 *
 * @param   {string} mgrsGridRef - String representation of MGRS grid reference.
 * @returns {Mgrs}   Mgrs grid reference object.
 * @throws  {Error}  Invalid MGRS grid reference.
 *
 * @example
 *   var mgrsRef = Mgrs.parse('31U DQ 48251 11932');
 *   var mgrsRef = Mgrs.parse('31UDQ4825111932');
 *   //  mgrsRef: { zone:31, band:'U', e100k:'D', n100k:'Q', easting:48251, northing:11932 }
 */
Mgrs.parse = function (mgrsGridRef) {
    mgrsGridRef = mgrsGridRef.trim();

    // check for military-style grid reference with no separators
    if (!mgrsGridRef.match(/\s/)) {
        var en = mgrsGridRef.slice(5); // get easting/northing following zone/band/100ksq
        en = en.slice(0, en.length / 2) + ' ' + en.slice(-en.length / 2); // separate easting/northing
        mgrsGridRef = mgrsGridRef.slice(0, 3) + ' ' + mgrsGridRef.slice(3, 5) + ' ' + en; // insert spaces
    }

    // match separate elements (separated by whitespace)
    mgrsGridRef = mgrsGridRef.match(/\S+/g);

    if (mgrsGridRef == null || mgrsGridRef.length != 4) throw new Error('Invalid MGRS grid reference ‘' + mgrsGridRef + '’');

    // split gzd into zone/band
    var gzd = mgrsGridRef[0];
    var zone = gzd.slice(0, 2);
    var band = gzd.slice(2, 3);

    // split 100km letter-pair into e/n
    var en100k = mgrsGridRef[1];
    var e100k = en100k.slice(0, 1);
    var n100k = en100k.slice(1, 2);

    var e = mgrsGridRef[2], n = mgrsGridRef[3];

    // standardise to 10-digit refs - ie metres) (but only if < 10-digit refs, to allow decimals)
    e = e.length >= 5 ? e : (e + '00000').slice(0, 5);
    n = n.length >= 5 ? n : (n + '00000').slice(0, 5);

    return new Mgrs(zone, band, e100k, n100k, e, n);
};


/**
 * Returns a string representation of an MGRS grid reference.
 *
 * To distinguish from civilian UTM coordinate representations, no space is included within the
 * zone/band grid zone designator.
 *
 * Components are separated by spaces: for a military-style unseparated string, use
 * Mgrs.toString().replace(/ /g, '');
 *
 * Note that MGRS grid references get truncated, not rounded (unlike UTM coordinates).
 *
 * @param   {number} [digits=10] - Precision of returned grid reference (eg 4 = km, 10 = m).
 * @returns {string} This grid reference in standard format.
 * @throws  {Error}  Invalid precision.
 *
 * @example
 *   var mgrsStr = new Mgrs(31, 'U', 'D', 'Q', 48251, 11932).toString(); // '31U DQ 48251 11932'
 */
Mgrs.prototype.toString = function (digits) {
    digits = (digits === undefined) ? 10 : Number(digits);
    if ([2, 4, 6, 8, 10].indexOf(digits) == -1) throw new Error('Invalid precision ‘' + digits + '’');

    var zone = ('00' + this.zone).slice(-2); // ensure leading zero
    var band = this.band;

    var e100k = this.e100k;
    var n100k = this.n100k;

    // truncate to required precision
    var eRounded = Math.floor(this.easting / Math.pow(10, 5 - digits / 2));
    var nRounded = Math.floor(this.northing / Math.pow(10, 5 - digits / 2));

    // ensure leading zeros
    var easting = ('00000' + eRounded).slice(-digits / 2);
    var northing = ('00000' + nRounded).slice(-digits / 2);

    return zone + band + ' ' + e100k + n100k + ' ' + easting + ' ' + northing;
};


/* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  */
if (typeof module != 'undefined' && module.exports) module.exports = Mgrs; // ≡ export default Mgrs