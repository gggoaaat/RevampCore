using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Revamp.IO.DB.Binds.IO.Dynamic;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Revamp.IO.Helpers.Helpers
{
    public class Fetcher
    {

        public string Getstring(string text)
        {
            return text;
        }

        //TODO: Delete All References to This
        public object ListALL(IConnectToDB _Connect, string objectlayer)
        {

            DataTable dt = new DataTable();
            DictionaryHelper dictionary;

            switch (objectlayer.ToLower())
            {
                case "object":
                case "objects":
                    ObjectsHelper objects = new ObjectsHelper();
                    dt = objects.FindAll(_Connect);
                    break;
                case "identity":
                case "identities":
                case "id":
                    IdentityHelper identity = new IdentityHelper();
                    dt = identity.FindAll(_Connect);
                    break;
                case "core":
                case "cores":
                    CoreHelper core = new CoreHelper();
                    dt = core.FindAll(_Connect);
                    break;
                case "application":
                case "applications":
                    AppHelper application = new AppHelper();
                    dt = application.FindAll(_Connect);
                    break;
                case "stage":
                case "stages":
                    StagesHelper stages = new StagesHelper();
                    dt = stages.FindAll(_Connect);
                    break;
                case "grip":
                case "grips":
                    GripsHelper grips = new GripsHelper();
                    dt = grips.FindAll(_Connect);
                    break;
                case "objectset":
                case "objectsets":
                    ObjectSetsHelper objectsets = new ObjectSetsHelper();
                    dt = objectsets.FindAll(_Connect);
                    break;
                case "objectpropset":
                case "objectpropsets":
                    ObjectPropSetsHelper objectpropsets = new ObjectPropSetsHelper();
                    dt = objectpropsets.FindAll(_Connect);
                    break;
                case "tables":
                case "table":
                    dictionary = new DictionaryHelper();
                    dt = dictionary.FindAll(_Connect, "tables");
                    break;
                case "views":
                case "view":
                    dictionary = new DictionaryHelper();
                    dt = dictionary.FindAll(_Connect, "views");
                    break;
                case "primary":
                case "primary keys":
                case "primary key":
                case "pk":
                case "pkey":
                case "pkeys":
                    dictionary = new DictionaryHelper();
                    dt = dictionary.FindAll(_Connect, "pkeys");
                    break;
                case "foreign":
                case "foreign keys":
                case "foreign key":
                case "fk":
                case "fkey":
                case "fkeys":
                    dictionary = new DictionaryHelper();
                    dt = dictionary.FindAll(_Connect, "fkeys");
                    break;
                case "unqiue":
                case "unqiue keys":
                case "unqiue key":
                case "uk":
                case "ukey":
                case "ukeys":
                    dictionary = new DictionaryHelper();
                    dt = dictionary.FindAll(_Connect, "ukeys");
                    break;
            }

            ConvertData jsonstring = new ConvertData();

            var json = jsonstring.ConvertDataTabletoString(dt);

            var myojb = new object();

            object myObjs = new object();

            switch (objectlayer.ToLower())
            {
                case "object":
                case "objects":
                    myojb = new ViewObjectModel();
                    List<ViewObjectModel> myObjsObjects = new List<ViewObjectModel>();
                    myObjsObjects = JsonConvert.DeserializeObject<List<ViewObjectModel>>(json);
                    myObjs = myObjsObjects;
                    break;
                case "inditities":
                case "identity":
                case "id":
                    myojb = new ViewIdentityModel();
                    List<ViewIdentityModel> myID = new List<ViewIdentityModel>();
                    myID = JsonConvert.DeserializeObject<List<ViewIdentityModel>>(json);
                    myObjs = myID;
                    break;
                case "core":
                case "cores":
                    myojb = new ViewCoreModel();
                    List<ViewCoreModel> myObjsCore = new List<ViewCoreModel>();
                    myObjsCore = JsonConvert.DeserializeObject<List<ViewCoreModel>>(json);
                    myObjs = myObjsCore;
                    break;
                case "application":
                case "applications":
                    myojb = new ViewApplicationModel();
                    List<ViewApplicationModel> myObjsApplication = new List<ViewApplicationModel>();
                    myObjsApplication = JsonConvert.DeserializeObject<List<ViewApplicationModel>>(json);
                    myObjs = myObjsApplication;
                    break;
                case "stage":
                case "stages":
                    myojb = new ViewStageModel();
                    List<ViewStageModel> myObjsStage = new List<ViewStageModel>();
                    myObjsStage = JsonConvert.DeserializeObject<List<ViewStageModel>>(json);
                    myObjs = myObjsStage;
                    break;
                case "grip":
                case "grips":
                    myojb = new ViewGripModel();
                    List<ViewGripModel> myObjsGrip = new List<ViewGripModel>();
                    myObjsGrip = JsonConvert.DeserializeObject<List<ViewGripModel>>(json);
                    myObjs = myObjsGrip;
                    break;
                case "objectset":
                case "objectsets":
                    myojb = new ViewObjectSetModel();
                    List<ViewObjectSetModel> myObjsObjectSets = new List<ViewObjectSetModel>();
                    myObjsObjectSets = JsonConvert.DeserializeObject<List<ViewObjectSetModel>>(json);
                    myObjs = myObjsObjectSets;
                    break;
                case "objectpropset":
                case "objectpropsets":
                    myojb = new ViewObjectPropSetsModel();
                    List<ViewObjectPropSetsModel> myObjsObjectPropSets = new List<ViewObjectPropSetsModel>();
                    myObjsObjectPropSets = JsonConvert.DeserializeObject<List<ViewObjectPropSetsModel>>(json);
                    myObjs = myObjsObjectPropSets;
                    break;
                case "tables":
                case "table":
                    myojb = new ViewTablesDictionary();
                    List<ViewTablesDictionary> myTableDictionary = new List<ViewTablesDictionary>();
                    myTableDictionary = JsonConvert.DeserializeObject<List<ViewTablesDictionary>>(json);
                    myObjs = myTableDictionary;
                    break;
                case "views":
                case "view":
                    myojb = new ViewViewsDictionary();
                    List<ViewViewsDictionary> myViewDictionary = new List<ViewViewsDictionary>();
                    myViewDictionary = JsonConvert.DeserializeObject<List<ViewViewsDictionary>>(json);
                    myObjs = myViewDictionary;
                    break;
                case "primary":
                case "primary keys":
                case "primary key":
                case "pk":
                case "pkey":
                case "pkeys":
                    myojb = new ViewPrimaryKeysDictionary();
                    List<ViewPrimaryKeysDictionary> myPKDictionary = new List<ViewPrimaryKeysDictionary>();
                    myPKDictionary = JsonConvert.DeserializeObject<List<ViewPrimaryKeysDictionary>>(json);
                    myObjs = myPKDictionary;
                    break;
                case "foreign":
                case "foreign keys":
                case "foreign key":
                case "fk":
                case "fkey":
                case "fkeys":
                    myojb = new ViewForeignKeysDictionary();
                    List<ViewForeignKeysDictionary> myFKDictionary = new List<ViewForeignKeysDictionary>();
                    myFKDictionary = JsonConvert.DeserializeObject<List<ViewForeignKeysDictionary>>(json);
                    myObjs = myFKDictionary;
                    break;
                case "unqiue":
                case "unqiue keys":
                case "unqiue key":
                case "uk":
                case "ukey":
                case "ukeys":
                    myojb = new ViewUniqueKeysDictionary();
                    List<ViewUniqueKeysDictionary> myUKDictionary = new List<ViewUniqueKeysDictionary>();
                    myUKDictionary = JsonConvert.DeserializeObject<List<ViewUniqueKeysDictionary>>(json);
                    myObjs = myUKDictionary;
                    break;
                default:
                    List<object> defaultlist = new List<object>();
                    defaultlist = null;
                    myObjs = defaultlist;
                    break;

            }

            return myObjs;
        }

        public object Find(IConnectToDB _Connect, string objectlayer, string _id)
        {

            DataTable dt = new DataTable();
            DictionaryHelper dictionary;

            switch (objectlayer.ToLower())
            {
                case "identity":
                case "identities":
                case "id":
                    IdentityHelper identity = new IdentityHelper();
                    dt = identity.Find(_Connect, _id);
                    break;
                case "core":
                case "cores":
                    CoreHelper core = new CoreHelper();
                    dt = core.Find(_Connect, _id);
                    break;
                case "application":
                case "applications":
                    AppHelper application = new AppHelper();
                    dt = application.FindByApplicationID(_Connect, _id);
                    break;
                case "stage":
                case "stages":
                    StagesHelper stages = new StagesHelper();
                    dt = stages.Find(_Connect, _id);
                    break;
                case "grip":
                case "grips":
                    GripsHelper grips = new GripsHelper();
                    dt = grips.Find(_Connect, _id);
                    break;
                case "objectset":
                case "objectsets":
                    ObjectSetsHelper objectsets = new ObjectSetsHelper();
                    dt = objectsets.Find(_Connect, _id);
                    break;
                case "objectpropset":
                case "objectpropsets":
                    ObjectPropSetsHelper objectpropsets = new ObjectPropSetsHelper();
                    dt = objectpropsets.Find(_Connect, _id);
                    break;
                case "tables":
                case "table":
                    dictionary = new DictionaryHelper();
                    dt = dictionary.Find(_Connect, "tables", _id);
                    break;
                case "views":
                case "view":
                    dictionary = new DictionaryHelper();
                    dt = dictionary.Find(_Connect, "views", _id);
                    break;
                case "primary":
                case "primary keys":
                case "primary key":
                case "pk":
                case "pkey":
                case "pkeys":
                    dictionary = new DictionaryHelper();
                    dt = dictionary.Find(_Connect, "pkeys", _id);
                    break;
                case "foreign":
                case "foreign keys":
                case "foreign key":
                case "fk":
                case "fkey":
                case "fkeys":
                    dictionary = new DictionaryHelper();
                    dt = dictionary.Find(_Connect, "fkeys", _id);
                    break;
                case "unqiue":
                case "unqiue keys":
                case "unqiue key":
                case "uk":
                case "ukey":
                case "ukeys":
                    dictionary = new DictionaryHelper();
                    dt = dictionary.Find(_Connect, "ukeys", _id);
                    break;
            }

            ConvertData jsonstring = new ConvertData();

            var json = jsonstring.ConvertDataTabletoString(dt);

            var myojb = new object();

            object myObjs = new object();

            switch (objectlayer)
            {
                case "object":
                case "objects":
                    myojb = new ViewObjectModel();
                    List<ViewObjectModel> myObjsObjects = new List<ViewObjectModel>();
                    myObjsObjects = JsonConvert.DeserializeObject<List<ViewObjectModel>>(json);
                    myObjs = myObjsObjects;
                    break;
                case "identity":
                case "identities":
                case "id":
                    myojb = new ViewIdentityModel();
                    List<ViewIdentityModel> myID = new List<ViewIdentityModel>();
                    myID = JsonConvert.DeserializeObject<List<ViewIdentityModel>>(json);
                    myObjs = myID;
                    break;
                case "core":
                case "cores":
                    myojb = new ViewCoreModel();
                    List<ViewCoreModel> myObjsCore = new List<ViewCoreModel>();
                    myObjsCore = JsonConvert.DeserializeObject<List<ViewCoreModel>>(json);
                    myObjs = myObjsCore;
                    break;
                case "application":
                case "applications":
                    myojb = new ViewApplicationModel();
                    List<ViewApplicationModel> myObjsApplication = new List<ViewApplicationModel>();
                    myObjsApplication = JsonConvert.DeserializeObject<List<ViewApplicationModel>>(json);
                    myObjs = myObjsApplication;
                    break;
                case "stage":
                case "stages":
                    myojb = new ViewStageModel();
                    List<ViewStageModel> myObjsStage = new List<ViewStageModel>();
                    myObjsStage = JsonConvert.DeserializeObject<List<ViewStageModel>>(json);
                    myObjs = myObjsStage;
                    break;
                case "grip":
                case "grips":
                    myojb = new ViewGripModel();
                    List<ViewGripModel> myObjsGrip = new List<ViewGripModel>();
                    myObjsGrip = JsonConvert.DeserializeObject<List<ViewGripModel>>(json);
                    myObjs = myObjsGrip;
                    break;
                case "objectset":
                case "objectsets":
                    myojb = new ViewObjectSetModel();
                    List<ViewObjectSetModel> myObjsObjectSets = new List<ViewObjectSetModel>();
                    myObjsObjectSets = JsonConvert.DeserializeObject<List<ViewObjectSetModel>>(json);
                    myObjs = myObjsObjectSets;
                    break;
                case "objectpropset":
                case "objectpropsets":
                    myojb = new ViewObjectPropSetsModel();
                    List<ViewObjectPropSetsModel> myObjsObjectpropSets = new List<ViewObjectPropSetsModel>();
                    myObjsObjectpropSets = JsonConvert.DeserializeObject<List<ViewObjectPropSetsModel>>(json);
                    myObjs = myObjsObjectpropSets;
                    break;
                case "tables":
                case "table":
                    myojb = new ViewTablesDictionary();
                    List<ViewTablesDictionary> myTableDictionary = new List<ViewTablesDictionary>();
                    myTableDictionary = JsonConvert.DeserializeObject<List<ViewTablesDictionary>>(json);
                    myObjs = myTableDictionary;
                    break;
                case "views":
                case "view":
                    myojb = new ViewViewsDictionary();
                    List<ViewViewsDictionary> myViewDictionary = new List<ViewViewsDictionary>();
                    myViewDictionary = JsonConvert.DeserializeObject<List<ViewViewsDictionary>>(json);
                    myObjs = myViewDictionary;
                    break;
                case "primary":
                case "primary keys":
                case "primary key":
                case "pk":
                case "pkey":
                case "pkeys":
                    myojb = new ViewPrimaryKeysDictionary();
                    List<ViewPrimaryKeysDictionary> myPKDictionary = new List<ViewPrimaryKeysDictionary>();
                    myPKDictionary = JsonConvert.DeserializeObject<List<ViewPrimaryKeysDictionary>>(json);
                    myObjs = myPKDictionary;
                    break;
                case "foreign":
                case "foreign keys":
                case "foreign key":
                case "fk":
                case "fkey":
                case "fkeys":
                    myojb = new ViewForeignKeysDictionary();
                    List<ViewForeignKeysDictionary> myFKDictionary = new List<ViewForeignKeysDictionary>();
                    myFKDictionary = JsonConvert.DeserializeObject<List<ViewForeignKeysDictionary>>(json);
                    myObjs = myFKDictionary;
                    break;
                case "unqiue":
                case "unqiue keys":
                case "unqiue key":
                case "uk":
                case "ukey":
                case "ukeys":
                    myojb = new ViewUniqueKeysDictionary();
                    List<ViewUniqueKeysDictionary> myUKDictionary = new List<ViewUniqueKeysDictionary>();
                    myUKDictionary = JsonConvert.DeserializeObject<List<ViewUniqueKeysDictionary>>(json);
                    myObjs = myUKDictionary;
                    break;
                default:
                    List<object> defaultlist = new List<object>();
                    defaultlist = null;
                    myObjs = defaultlist;
                    break;

            }

            return myObjs;
        }

        public object FindbyColumnID(IConnectToDB _Connect, string objectlayer, string _column, string _id)
        {
            DataTable dt = new DataTable();
            DictionaryHelper dictionary;

            switch (objectlayer.ToLower())
            {

                case "object":
                case "objects":
                    ObjectsHelper objects = new ObjectsHelper();
                    dt = objects.FindbyColumnID(_Connect, _column, _id);
                    break;
                case "identity":
                case "identities":
                case "id":
                    IdentityHelper identity = new IdentityHelper();
                    dt = identity.FindbyColumnID(_Connect, _column, _id);
                    break;
                case "core":
                case "cores":
                    CoreHelper core = new CoreHelper();
                    dt = core.FindbyColumnID(_Connect, _column, _id);
                    break;
                case "application":
                case "applications":
                    AppHelper application = new AppHelper();
                    dt = application.FindbyColumnID(_Connect, _column, _id);
                    break;
                case "stage":
                case "stages":
                    StagesHelper stages = new StagesHelper();
                    dt = stages.FindbyColumnID(_Connect, _column, _id);
                    break;
                case "grip":
                case "grips":
                    GripsHelper grips = new GripsHelper();
                    dt = grips.FindbyColumnID(_Connect, _column, _id);
                    break;
                case "objectset":
                case "objectsets":
                    ObjectSetsHelper objectsets = new ObjectSetsHelper();
                    dt = objectsets.FindbyColumnID(_Connect, _column, _id);
                    break;
                case "objectpropset":
                case "objectpropsets":
                    ObjectPropSetsHelper objectpropsets = new ObjectPropSetsHelper();
                    dt = objectpropsets.FindbyColumnID(_Connect, _column, _id);
                    break;
                case "tables":
                case "table":
                    dictionary = new DictionaryHelper();
                    dt = dictionary.FindbyColumnID(_Connect, "tables", _column, _id);
                    break;
                case "views":
                case "view":
                    dictionary = new DictionaryHelper();
                    dt = dictionary.FindbyColumnID(_Connect, "views", _column, _id);
                    break;
                case "primary":
                case "primary keys":
                case "primary key":
                case "pk":
                case "pkey":
                case "pkeys":
                    dictionary = new DictionaryHelper();
                    dt = dictionary.FindbyColumnID(_Connect, "pkeys", _column, _id);
                    break;
                case "foreign":
                case "foreign keys":
                case "foreign key":
                case "fk":
                case "fkey":
                case "fkeys":
                    dictionary = new DictionaryHelper();
                    dt = dictionary.FindbyColumnID(_Connect, "fkeys", _column, _id);
                    break;
                case "unqiue":
                case "unqiue keys":
                case "unqiue key":
                case "uk":
                case "ukey":
                case "ukeys":
                    dictionary = new DictionaryHelper();
                    dt = dictionary.FindbyColumnID(_Connect, "ukeys", _column, _id);
                    break;
            }

            ConvertData jsonstring = new ConvertData();

            var json = jsonstring.ConvertDataTabletoString(dt);

            var myojb = new object();

            object myObjs = new object();

            switch (objectlayer)
            {
                case "object":
                case "objects":
                    myojb = new ViewObjectModel();
                    List<ViewObjectModel> myObjsObjects = new List<ViewObjectModel>();
                    myObjsObjects = JsonConvert.DeserializeObject<List<ViewObjectModel>>(json);
                    myObjs = myObjsObjects;
                    break;
                case "identity":
                case "identities":
                case "id":
                    myojb = new ViewIdentityModel();
                    List<ViewIdentityModel> myID = new List<ViewIdentityModel>();
                    myID = JsonConvert.DeserializeObject<List<ViewIdentityModel>>(json);
                    myObjs = myID;
                    break;
                case "core":
                case "cores":
                    myojb = new ViewCoreModel();
                    List<ViewCoreModel> myObjsCore = new List<ViewCoreModel>();
                    myObjsCore = JsonConvert.DeserializeObject<List<ViewCoreModel>>(json);
                    myObjs = myObjsCore;
                    break;
                case "application":
                case "applications":
                    myojb = new ViewApplicationModel();
                    List<ViewApplicationModel> myObjsApplication = new List<ViewApplicationModel>();
                    myObjsApplication = JsonConvert.DeserializeObject<List<ViewApplicationModel>>(json);
                    myObjs = myObjsApplication;
                    break;
                case "stage":
                case "stages":
                    myojb = new ViewStageModel();
                    List<ViewStageModel> myObjsStage = new List<ViewStageModel>();
                    myObjsStage = JsonConvert.DeserializeObject<List<ViewStageModel>>(json);
                    myObjs = myObjsStage;
                    break;
                case "grip":
                case "grips":
                    myojb = new ViewGripModel();
                    List<ViewGripModel> myObjsGrip = new List<ViewGripModel>();
                    myObjsGrip = JsonConvert.DeserializeObject<List<ViewGripModel>>(json);
                    myObjs = myObjsGrip;
                    break;
                case "objectset":
                case "objectsets":
                    myojb = new ViewObjectSetModel();
                    List<ViewObjectSetModel> myObjsObjectSets = new List<ViewObjectSetModel>();
                    myObjsObjectSets = JsonConvert.DeserializeObject<List<ViewObjectSetModel>>(json);
                    myObjs = myObjsObjectSets;
                    break;
                case "objectpropset":
                case "objectpropsets":
                    myojb = new ViewObjectPropSetsModel();
                    List<ViewObjectPropSetsModel> myObjsObjectPropSets = new List<ViewObjectPropSetsModel>();
                    myObjsObjectPropSets = JsonConvert.DeserializeObject<List<ViewObjectPropSetsModel>>(json);
                    myObjs = myObjsObjectPropSets;
                    break;
                case "tables":
                case "table":
                    myojb = new ViewTablesDictionary();
                    List<ViewTablesDictionary> myTableDictionary = new List<ViewTablesDictionary>();
                    myTableDictionary = JsonConvert.DeserializeObject<List<ViewTablesDictionary>>(json);
                    myObjs = myTableDictionary;
                    break;
                case "views":
                case "view":
                    myojb = new ViewViewsDictionary();
                    List<ViewViewsDictionary> myViewDictionary = new List<ViewViewsDictionary>();
                    myViewDictionary = JsonConvert.DeserializeObject<List<ViewViewsDictionary>>(json);
                    myObjs = myViewDictionary;
                    break;
                case "primary":
                case "primary keys":
                case "primary key":
                case "pk":
                case "pkey":
                case "pkeys":
                    myojb = new ViewPrimaryKeysDictionary();
                    List<ViewPrimaryKeysDictionary> myPKDictionary = new List<ViewPrimaryKeysDictionary>();
                    myPKDictionary = JsonConvert.DeserializeObject<List<ViewPrimaryKeysDictionary>>(json);
                    myObjs = myPKDictionary;
                    break;
                case "foreign":
                case "foreign keys":
                case "foreign key":
                case "fk":
                case "fkey":
                case "fkeys":
                    myojb = new ViewForeignKeysDictionary();
                    List<ViewForeignKeysDictionary> myFKDictionary = new List<ViewForeignKeysDictionary>();
                    myFKDictionary = JsonConvert.DeserializeObject<List<ViewForeignKeysDictionary>>(json);
                    myObjs = myFKDictionary;
                    break;
                case "unqiue":
                case "unqiue keys":
                case "unqiue key":
                case "uk":
                case "ukey":
                case "ukeys":
                    myojb = new ViewUniqueKeysDictionary();
                    List<ViewUniqueKeysDictionary> myUKDictionary = new List<ViewUniqueKeysDictionary>();
                    myUKDictionary = JsonConvert.DeserializeObject<List<ViewUniqueKeysDictionary>>(json);
                    myObjs = myUKDictionary;
                    break;
                default:
                    List<object> defaultlist = new List<object>();
                    defaultlist = null;
                    myObjs = defaultlist;
                    break;

            }

            return myObjs;
        }

        public object FindbyColumnIDs(IConnectToDB _Connect, string objectlayer, string _column, List<string> _ids)
        {
            DataTable dt = new DataTable();
            DictionaryHelper dictionary;

            switch (objectlayer.ToLower())
            {
                case "object":
                case "objects":
                    ObjectsHelper objects = new ObjectsHelper();
                    dt = objects.FindbyColumnIDs(_Connect, _column, _ids);
                    break;
                case "identity":
                case "identities":
                case "id":
                    IdentityHelper identity = new IdentityHelper();
                    dt = identity.FindbyColumnIDs(_Connect, _column, _ids);
                    break;
                case "core":
                case "cores":
                    CoreHelper core = new CoreHelper();
                    dt = core.FindbyColumnIDs(_Connect, _column, _ids);
                    break;
                case "application":
                case "applications":
                    AppHelper application = new AppHelper();
                    dt = application.FindbyColumnIDs(_Connect, _column, _ids);
                    break;
                case "stage":
                case "stages":
                    StagesHelper stages = new StagesHelper();
                    dt = stages.FindbyColumnIDs(_Connect, _column, _ids);
                    break;
                case "grip":
                case "grips":
                    GripsHelper grips = new GripsHelper();
                    dt = grips.FindbyColumnIDs(_Connect, _column, _ids);
                    break;
                case "objectset":
                case "objectsets":
                    ObjectSetsHelper objectsets = new ObjectSetsHelper();
                    dt = objectsets.FindbyColumnIDs(_Connect, _column, _ids);
                    break;
                case "objectpropset":
                case "objectpropsets":
                    ObjectPropSetsHelper objectpropsets = new ObjectPropSetsHelper();
                    dt = objectpropsets.FindbyColumnIDs(_Connect, _column, _ids);
                    break;
                case "tables":
                case "table":
                    dictionary = new DictionaryHelper();
                    dt = dictionary.FindbyColumnIDs(_Connect, "tables", _column, _ids);
                    break;
                case "views":
                case "view":
                    dictionary = new DictionaryHelper();
                    dt = dictionary.FindbyColumnIDs(_Connect, "views", _column, _ids);
                    break;
                case "primary":
                case "primary keys":
                case "primary key":
                case "pk":
                case "pkey":
                case "pkeys":
                    dictionary = new DictionaryHelper();
                    dt = dictionary.FindbyColumnIDs(_Connect, "pkeys", _column, _ids);
                    break;
                case "foreign":
                case "foreign keys":
                case "foreign key":
                case "fk":
                case "fkey":
                case "fkeys":
                    dictionary = new DictionaryHelper();
                    dt = dictionary.FindbyColumnIDs(_Connect, "fkeys", _column, _ids);
                    break;
                case "unqiue":
                case "unqiue keys":
                case "unqiue key":
                case "uk":
                case "ukey":
                case "ukeys":
                    dictionary = new DictionaryHelper();
                    dt = dictionary.FindbyColumnIDs(_Connect, "ukeys", _column, _ids);
                    break;
            }

            ConvertData jsonstring = new ConvertData();

            var json = jsonstring.ConvertDataTabletoString(dt);

            var myojb = new object();

            object myObjs = new object();

            switch (objectlayer)
            {
                case "object":
                case "objects":
                    myojb = new ViewObjectModel();
                    List<ViewObjectModel> myObjsObjects = new List<ViewObjectModel>();
                    myObjsObjects = JsonConvert.DeserializeObject<List<ViewObjectModel>>(json);
                    myObjs = myObjsObjects;
                    break;
                case "identity":
                case "identities":
                case "id":
                    myojb = new ViewIdentityModel();
                    List<ViewIdentityModel> myID = new List<ViewIdentityModel>();
                    myID = JsonConvert.DeserializeObject<List<ViewIdentityModel>>(json);
                    myObjs = myID;
                    break;
                case "core":
                case "cores":
                    myojb = new ViewCoreModel();
                    List<ViewCoreModel> myObjsCore = new List<ViewCoreModel>();
                    myObjsCore = JsonConvert.DeserializeObject<List<ViewCoreModel>>(json);
                    myObjs = myObjsCore;
                    break;
                case "application":
                case "applications":
                    myojb = new ViewApplicationModel();
                    List<ViewApplicationModel> myObjsApplication = new List<ViewApplicationModel>();
                    myObjsApplication = JsonConvert.DeserializeObject<List<ViewApplicationModel>>(json);
                    myObjs = myObjsApplication;
                    break;
                case "stage":
                case "stages":
                    myojb = new ViewStageModel();
                    List<ViewStageModel> myObjsStage = new List<ViewStageModel>();
                    myObjsStage = JsonConvert.DeserializeObject<List<ViewStageModel>>(json);
                    myObjs = myObjsStage;
                    break;
                case "grip":
                case "grips":
                    myojb = new ViewGripModel();
                    List<ViewGripModel> myObjsGrip = new List<ViewGripModel>();
                    myObjsGrip = JsonConvert.DeserializeObject<List<ViewGripModel>>(json);
                    myObjs = myObjsGrip;
                    break;
                case "objectset":
                case "objectsets":
                    myojb = new ViewObjectSetModel();
                    List<ViewObjectSetModel> myObjsObjectSets = new List<ViewObjectSetModel>();
                    myObjsObjectSets = JsonConvert.DeserializeObject<List<ViewObjectSetModel>>(json);
                    myObjs = myObjsObjectSets;
                    break;
                case "objectpropset":
                case "objectpropsets":
                    myojb = new ViewObjectPropSetsModel();
                    List<ViewObjectPropSetsModel> myObjsObjectPropSets = new List<ViewObjectPropSetsModel>();
                    myObjsObjectPropSets = JsonConvert.DeserializeObject<List<ViewObjectPropSetsModel>>(json);
                    myObjs = myObjsObjectPropSets;
                    break;
                case "tables":
                case "table":
                    myojb = new ViewTablesDictionary();
                    List<ViewTablesDictionary> myTableDictionary = new List<ViewTablesDictionary>();
                    myTableDictionary = JsonConvert.DeserializeObject<List<ViewTablesDictionary>>(json);
                    myObjs = myTableDictionary;
                    break;
                case "views":
                case "view":
                    myojb = new ViewViewsDictionary();
                    List<ViewViewsDictionary> myViewDictionary = new List<ViewViewsDictionary>();
                    myViewDictionary = JsonConvert.DeserializeObject<List<ViewViewsDictionary>>(json);
                    myObjs = myViewDictionary;
                    break;
                case "primary":
                case "primary keys":
                case "primary key":
                case "pk":
                case "pkey":
                case "pkeys":
                    myojb = new ViewPrimaryKeysDictionary();
                    List<ViewPrimaryKeysDictionary> myPKDictionary = new List<ViewPrimaryKeysDictionary>();
                    myPKDictionary = JsonConvert.DeserializeObject<List<ViewPrimaryKeysDictionary>>(json);
                    myObjs = myPKDictionary;
                    break;
                case "foreign":
                case "foreign keys":
                case "foreign key":
                case "fk":
                case "fkey":
                case "fkeys":
                    myojb = new ViewForeignKeysDictionary();
                    List<ViewForeignKeysDictionary> myFKDictionary = new List<ViewForeignKeysDictionary>();
                    myFKDictionary = JsonConvert.DeserializeObject<List<ViewForeignKeysDictionary>>(json);
                    myObjs = myFKDictionary;
                    break;
                case "unqiue":
                case "unqiue keys":
                case "unqiue key":
                case "uk":
                case "ukey":
                case "ukeys":
                    myojb = new ViewUniqueKeysDictionary();
                    List<ViewUniqueKeysDictionary> myUKDictionary = new List<ViewUniqueKeysDictionary>();
                    myUKDictionary = JsonConvert.DeserializeObject<List<ViewUniqueKeysDictionary>>(json);
                    myObjs = myUKDictionary;
                    break;
                default:
                    List<object> defaultlist = new List<object>();
                    defaultlist = null;
                    myObjs = defaultlist;
                    break;

            }

            return myObjs;
        }

        public List<string> GetAllStagesforApp(IConnectToDB _Connect, string applications_id)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "APPLICATIONS_ID_", DBType = SqlDbType.BigInt, ParamValue = applications_id });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__STAGES_SEARCH",
                new DataTableDotNetModelMetaData { columns = "stages_id", length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            List<string> stagesList = new List<string>();

            foreach (DataRow datarowdc in TempDataTable.Rows)
            {
                stagesList.Add(datarowdc["stages_id"].ToString());
            }

            return stagesList;
        }     

        public List<ViewObjectPropSetsModel> GetObjectPropSetsViaObjectSet(IConnectToDB _Connect, string object_sets_id)
        {
            //ObjectSets objectsets = new ObjectSets();
            ObjectPropSetsHelper objectpropsets = new ObjectPropSetsHelper();

            DataTable objectpropsetsdt;

            if (object_sets_id.ToLower() == "all")
            {
                objectpropsetsdt = objectpropsets.FindAll(_Connect);
            }
            else
            {
                objectpropsetsdt = objectpropsets.FindbyColumnID(_Connect, "object_sets_id", object_sets_id);
            }

            List<ViewObjectPropSetsModel> ObjectPropSetsList = new List<ViewObjectPropSetsModel>();

            ViewObjectPropSetsModel[] ObjectPropSets = new ViewObjectPropSetsModel[objectpropsetsdt.Rows.Count];


            for (int i = 0; i < objectpropsetsdt.Rows.Count; i++)
            {

                string thisPropSet = new JObject(objectpropsetsdt.Columns.Cast<DataColumn>()
                                         .Select(c => new JProperty(c.ColumnName, JToken.FromObject(objectpropsetsdt.Rows[i][c])))
                                   ).ToString(Formatting.None);

                ObjectPropSets[i] = JsonConvert.DeserializeObject<ViewObjectPropSetsModel>(thisPropSet);

                ObjectPropSets[i].ObjectPropOptSets = GetPropOptSetsViaPropSets(_Connect, ObjectPropSets[i].obj_prop_sets_id, _Connect.SourceDBOwner);

                //ObjectPropSets[i].stage_name = datarowdc["stage_name"].ToString();
                ObjectPropSetsList.Add(ObjectPropSets[i]);

            }

            return ObjectPropSetsList;
        }

        public List<ViewObjectPropOptSetsModel> GetPropOptSetsViaPropSets(IConnectToDB _Connect, long? obj_prop_sets_id, string connownername)
        {
            ObjectPropOptSets objectpropoptsets = new ObjectPropOptSets();

            DataTable objectpropoptsetsdt = objectpropoptsets.FindbyColumnID(_Connect, "obj_prop_sets_id", obj_prop_sets_id.ToString());

            List<ViewObjectPropOptSetsModel> ObjectPropOptSetsList = new List<ViewObjectPropOptSetsModel>();

            ViewObjectPropOptSetsModel[] ObjectPropOptSets = new ViewObjectPropOptSetsModel[objectpropoptsetsdt.Rows.Count];

            for (int i = 0; i < objectpropoptsetsdt.Rows.Count; i++)
            {
                string thisPropOptSet = new JObject(objectpropoptsetsdt.Columns.Cast<DataColumn>()
                                        .Select(c => new JProperty(c.ColumnName, JToken.FromObject(objectpropoptsetsdt.Rows[i][c])))
                                  ).ToString(Formatting.None);

                ObjectPropOptSets[i] = JsonConvert.DeserializeObject<ViewObjectPropOptSetsModel>(thisPropOptSet);

                ObjectPropOptSetsList.Add(ObjectPropOptSets[i]);
            }

            return ObjectPropOptSetsList;
        }

        public List<ViewIdentityModel> GetIdentityViaID(IConnectToDB _Connect, string _id, string connownername)
        {
            //ViewIdentityModel appObjects = new ViewIdentityModel();

            IdentityHelper identity = new IdentityHelper();

            DataTable identitydt;

            if (_id.ToLower() == "all")
            {
                identitydt = identity.FindAll(_Connect);
            }
            else
            {
                identitydt = identity.FindbyColumnID(_Connect, "identities_id", _id);
            }

            List<ViewIdentityModel> IdentitiesList = new List<ViewIdentityModel>();

            ViewIdentityModel[] Identities = new ViewIdentityModel[identitydt.Rows.Count];

            int i = 0;
            foreach (DataRow datarowdc in identitydt.Rows)
            {

                Identities[i] = new ViewIdentityModel
                {

                    identities_id = datarowdc.Field<long?>("identities_id"),
                    enabled = datarowdc.Field<string>("enabled"),
                    dt_created = datarowdc.Field<DateTime>("dt_created"),
                    dt_available = datarowdc.Field<DateTime?>("dt_available"),
                    dt_end = datarowdc.Field<DateTime?>("dt_end"),
                    object_type = datarowdc.Field<string>("object_type"),
                    object_layer = datarowdc.Field<string>("object_layer"),
                    chardata = new List<ViewIdentityCharDataModel>(), //Still need to build out Helpers
                    datedata = new List<ViewIdentityDateDataModel>(), //Still need to build out Helpers
                    numbdata = new List<ViewIdentityNumbDataModel>() //Still need to build out Helpers

                };
                //Grips[i].stage_name = datarowdc["stage_name"].ToString();
                IdentitiesList.Add(Identities[i]);
                i++;
            }

            return IdentitiesList;
        }


    }
}