var Base64 = {}; //Base 64 namespace

Base64.code = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";


Base64.encode = function (str, utf8encode) { //http://tools.ietf.org/html/rfc4648
    utf8encode = (typeof utf8encode == 'undefined') ? false : utf8encode;
    var o1, o2, o3, bits, h1, h2, h3, h4, e = [], pad = '', c, plain, coded;
    var b64 = Base64.code;

    plain = utf8encode ? Utf8.encode(str) : str;

    c = plain.length % 3; //pad string to length of multiple of 3

    if (c > 0) { while (c++ < 3) { pad += '='; plain += '\0'; } }
    //note: doing padding here saves us doing special-case packin for trailing 1 or 2 chars

    for (c = 0; c < plain.length; c += 3) { //pack three octects into four hexets
        o1 = plain.charCodeAt(c);
        o2 = plain.charCodeAt(c + 1);
        o3 = plain.charCodeAt(c + 2);

        bits = o1 << 16 | o2 << 8 | o3;

        h1 = bits >> 18 & 0x3f;
        h2 = bits >> 12 & 0x3f;
        h3 = bits >> 6 & 0x3f;
        h4 = bits & 0x3f;

        //use hextets to index into code string
        e[c / 3] = b64.charAt(h1) + b64.charAt(h2) + b64.charAt(h3) + b64.charAt(h4);
    }

    coded = e.join(''); // join() is far faster than repeated string concatenation in IE

    //replace 'A's from padded  nulls with '='s
    coded = coded.slice(0, coded.length - pad.length) + pad;

    return coded;

}

Base64.decode = function (str, utf8decode) {
    utf8decode = (typeof utf8decode == 'undefined') ? false : utf8decode;

    var o1, o2, o3, bits, h1, h2, h3, h4, d = [], c, plain, coded;
    var b64 = Base64.code;

    coded = utf8decode ? Utf8.decode(str) : str;

    for (var c = 0; c < coded.length; c += 4) {
        h1 = b64.indexOf(coded.charAt(c));
        h2 = b64.indexOf(coded.charAt(c + 1));
        h3 = b64.indexOf(coded.charAt(c + 2));
        h4 = b64.indexOf(coded.charAt(c + 3));

        bits = h1 << 18 | h2 << 12 | h3 << 6 | h4

        o1 = bits >>> 16 & 0xff;
        o2 = bits >>> 8 & 0xff;
        o3 = bits & 0xff;

        d[c / 4] = String.fromCharCode(o1, o2, o3);
        // check for padding

        if (h4 == 0x40) d[c / 4] = String.fromCharCode(o1, o2);
        if (h3 == 0x40) d[c / 4] = String.fromCharCode(o1);
    }

    plain = d.join(''); //join() is far faster than repeated string concatenation in IE

    return utf8decode ? Utf8.decode(plain) : plain;
}

Base64.isThisEncoded = function (str) {
    var base64regex = /^([0-9a-zA-Z+/]{4})*(([0-9a-zA-Z+/]{2}==)|([0-9a-zA-Z+/]{3}=))?$/;

    return base64regex.test(str);
}
