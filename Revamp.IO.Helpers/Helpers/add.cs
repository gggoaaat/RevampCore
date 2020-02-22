using Revamp.IO.DB.Bridge;
using Revamp.IO.Foundation;
using Revamp.IO.Structs;
using Revamp.IO.Structs.Models;
using Revamp.IO.Structs.Models.DataEntry;
using System;
using System.Collections.Generic;

namespace Revamp.IO.Helpers.Helpers
{
    public class add
    {
        public T ADD_ENTRY_Generic<T>(IConnectToDB _Connect, T thisModel)
        {
            Universal_Call<T> universalCall = new Universal_Call<T>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public sqlTransBlocks ADD_ENTRY_Generic<T, T1>(IConnectToDB _Connect, T thisGenericSerie, T1 paramModel, sqlTransBlocks Serie)
        {
            TransactionCall<T, T1> universalCall = new TransactionCall<T, T1>();
            SQLProcedureModels.BIG_CALL params1 = universalCall.GetParams(thisGenericSerie, paramModel);

            Serie.Series.Add(new SQLTrasaction
            {
                ProcedureName = params1.COMMANDS[0].ProcedureName,
                EntryProcedureParameters = params1.COMMANDS[0]._dbParameters
            });

            return Serie;
        }

        #region Cores
        public Values.AddCore ADD_ENTRY_Cores(IConnectToDB _Connect, Values.AddCore thisModel)
        {
            Universal_Call<Values.AddCore> universalCall = new Universal_Call<Values.AddCore>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;

        }

        #endregion

        #region Applications
        //Add Application to Database
        public Values.AddApplication ADD_ENTRY_Application(IConnectToDB _Connect, Values.AddApplication thisModel)
        {
            Universal_Call<Values.AddApplication> universalCall = new Universal_Call<Values.AddApplication>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        

        public sqlTransBlocks ADD_ENTRY_Application(IConnectToDB _Connect, CASTGOOP.Application thisApplication, sqlTransBlocks Serie)
        {
            Values.AddApplication thisModel = new Values.AddApplication();

            TransactionCall<CASTGOOP.Application, Values.AddApplication> universalCall = new TransactionCall<CASTGOOP.Application, Values.AddApplication>();
            SQLProcedureModels.BIG_CALL params1 = universalCall.GetParams(thisApplication, thisModel);

            Serie.Series.Add(new SQLTrasaction
            {
                ProcedureName = params1.COMMANDS[0].ProcedureName,
                EntryProcedureParameters = params1.COMMANDS[0]._dbParameters
            });

            return Serie;
        }
        #endregion

        #region Stages
        public CASTGOOP.Stage ADD_ENTRY_Stage(IConnectToDB _Connect, CASTGOOP.Stage thisModel)
        {
            Universal_Call<CASTGOOP.Stage> universalCall = new Universal_Call<CASTGOOP.Stage>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public Values.AddStage ADD_ENTRY_Stage(IConnectToDB _Connect, Values.AddStage thisModel) //, string StageType, string StageName, string APP_ID, string CONTAINERS_ID, string Identities_ID, string StageLink)
        {
            Universal_Call<Values.AddStage> universalCall = new Universal_Call<Values.AddStage>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);
            return thisModel;
        }

        public sqlTransBlocks ADD_ENTRY_Stage(IConnectToDB _Connect, CASTGOOP.Stage thisStage, sqlTransBlocks Serie)
        {
            Values.AddStage thisModel = new Values.AddStage();

            TransactionCall<CASTGOOP.Stage, Values.AddStage> universalCall = new TransactionCall<CASTGOOP.Stage, Values.AddStage>();
            SQLProcedureModels.BIG_CALL params1 = universalCall.GetParams(thisStage, thisModel);

            Serie.Series.Add(new SQLTrasaction
            {
                ProcedureName = params1.COMMANDS[0].ProcedureName,
                EntryProcedureParameters = params1.COMMANDS[0]._dbParameters
            });

            return Serie;
        }
        #endregion

        #region Grips
        //ADD Grip to the Database.
        //Grip Belong to Stages

        public sqlTransBlocks Grip(IConnectToDB _Connect, CASTGOOP.Grip thisGrip, sqlTransBlocks Serie)
        {
            Values.AddGrip thisModel = new Values.AddGrip();

            TransactionCall<CASTGOOP.Grip, Values.AddGrip> universalCall = new TransactionCall<CASTGOOP.Grip, Values.AddGrip>();
            SQLProcedureModels.BIG_CALL params1 = universalCall.GetParams(thisGrip, thisModel);

            Serie.Series.Add(new SQLTrasaction
            {
                ProcedureName = params1.COMMANDS[0].ProcedureName,
                EntryProcedureParameters = params1.COMMANDS[0]._dbParameters
            });

            return Serie;
        }

        public Values.AddGrip ADD_ENTRY_Grip(IConnectToDB _Connect, Values.AddGrip thisModel)
        {
            Universal_Call<Values.AddGrip> universalCall = new Universal_Call<Values.AddGrip>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }
        #endregion

        #region Object Set
        //ADD Object Set to Database
        //Object Sets Belong to Grips
        public Values.AddObjectSet ADD_ENTRY_Object_Set(IConnectToDB _Connect, Values.AddObjectSet thisModel)
        {
            Universal_Call<Values.AddObjectSet> universalCall = new Universal_Call<Values.AddObjectSet>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public sqlTransBlocks ObjectSet(IConnectToDB _Connect, CASTGOOP.ObjectSet thisObjectSet, sqlTransBlocks Serie)
        {
            //Universal_Call<CASTGOOP.ObjectSet> universalCall = new Universal_Call<CASTGOOP.ObjectSet>();
            //SQLTrasaction thisTran = universalCall.GenericInputSQLTransaction(_Connect, thisObjectSet);

            Values.AddObjectSet thisModel = new Values.AddObjectSet();

            TransactionCall<CASTGOOP.ObjectSet, Values.AddObjectSet> universalCall = new TransactionCall<CASTGOOP.ObjectSet, Values.AddObjectSet>();
            SQLProcedureModels.BIG_CALL params1 = universalCall.GetParams(thisObjectSet, thisModel);

            Serie.Series.Add(new SQLTrasaction
            {
                ProcedureName = params1.COMMANDS[0].ProcedureName,
                EntryProcedureParameters = params1.COMMANDS[0]._dbParameters
            });

            return Serie;
        }
        #endregion

        //TODO: Come back refactor.
        #region Object Set Property
        public sqlTransBlocks ObjectSetProperty(IConnectToDB _Connect, CASTGOOP.ObjectPropSets thisPropSet, sqlTransBlocks Serie  /*string _objectssetsid, string _objecttype, string _valuedatatype, string _propertyname, string _hasparent, string _haschild, string _parentobjectid, string _propertyvalue*/)
        {
            thisPropSet.I_PROPERTY_VALUE = string.IsNullOrWhiteSpace((string)thisPropSet.I_PROPERTY_VALUE) ? " " : thisPropSet.I_PROPERTY_VALUE;
            thisPropSet.I_PARENT_OBJ_PROP_SETS_ID = thisPropSet.I_PARENT_OBJ_PROP_SETS_ID == null ? 0 : thisPropSet.I_PARENT_OBJ_PROP_SETS_ID;

            Values.AddObjectPropertySet thisModel = new Values.AddObjectPropertySet();

            TransactionCall<CASTGOOP.ObjectPropSets, Values.AddObjectPropertySet> universalCall = new TransactionCall<CASTGOOP.ObjectPropSets, Values.AddObjectPropertySet>();
            SQLProcedureModels.BIG_CALL params1 = universalCall.GetParams(thisPropSet, thisModel);

            if (thisPropSet.V_AVOID_ANTIXSS)
            {
                params1.COMMANDS[0]._dbParameters.Find(r => r.ParamName == "@I_PROPERTY_VALUE").AvoidAntiXss = true;
            }

            Serie.Series.Add(new SQLTrasaction
            {
                ProcedureName = params1.COMMANDS[0].ProcedureName,
                EntryProcedureParameters = params1.COMMANDS[0]._dbParameters
            });

            return Serie;
        }

        public Values.AddObjectPropertySet ADD_ENTRY_Object_Property_Set(IConnectToDB _Connect, Values.AddObjectPropertySet thisModel) // string _ObjectsSetsID, string _ObjectType, string _ValueDatatype, string _PropertyName, string _HasParent, string _HasChild, string _ParentObjectPropID, string _PropertyValue)
        {
            Universal_Call<Values.AddObjectPropertySet> universalCall = new Universal_Call<Values.AddObjectPropertySet>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public Values.AddObjectPropertySet addObjectSetProperty(IConnectToDB _Connect, Values.AddObjectPropertySet thisModel  /*string _ObjectsSetsID, string _ObjectType, string _ValueDatatype, string _PropertyName, string _HasParent, string _HasChild, string _ParentObjectPropID, string _PropertyValue*/)
        {
            Universal_Call<Values.AddObjectPropertySet> universalCall = new Universal_Call<Values.AddObjectPropertySet>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        #endregion

        #region Object Set Property Option
        public sqlTransBlocks ObjectSetPropertyOption(IConnectToDB _Connect, CASTGOOP.ObjectPropSetOption thisObjectPropSetOption, sqlTransBlocks Serie)
        {
            Values.AddObjectPropertyOption thisModel = new Values.AddObjectPropertyOption();

            TransactionCall<CASTGOOP.ObjectPropSetOption, Values.AddObjectPropertyOption> universalCall = new TransactionCall<CASTGOOP.ObjectPropSetOption, Values.AddObjectPropertyOption>();
            SQLProcedureModels.BIG_CALL params1 = universalCall.GetParams(thisObjectPropSetOption, thisModel);

            Serie.Series.Add(new SQLTrasaction
            {
                ProcedureName = params1.COMMANDS[0].ProcedureName,
                EntryProcedureParameters = params1.COMMANDS[0]._dbParameters
            });

            return Serie;
        }

        public Values.AddObjectPropertyOption addObjectSetPropertyOption(IConnectToDB _Connect, Values.AddObjectPropertyOption thisModel)
        {
            Universal_Call<Values.AddObjectPropertyOption> universalCall = new Universal_Call<Values.AddObjectPropertyOption>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public static List<CommandResult> AddListOfPropOptionSets(IConnectToDB _Connect, List<Values.AddObjectPropertyOption> optionsList)
        {
            add addHelp = new add();
            List<CommandResult> Logger = new List<CommandResult>();

            for (int i = 0; i < optionsList.Count; i++)
            {
                optionsList[i] = addHelp.addObjectSetPropertyOption(_Connect, optionsList[i]);

                CommandResult thisObjectPropOptionResult = new CommandResult();
                thisObjectPropOptionResult.attemptedCommand = optionsList[i].V_ATTEMPTED_SQL;
                thisObjectPropOptionResult._Successful = optionsList[i].O_OBJ_PROP_OPT_SETS_ID > 0 ? true : false;
                thisObjectPropOptionResult._Response = thisObjectPropOptionResult._Successful ? "Added Object Property Option " + optionsList[i].I_OPTION_VALUE : "Failed to Add Object Property Set " + optionsList[i].I_OPTION_VALUE;
                thisObjectPropOptionResult._EndTime = DateTime.Now;
                Logger.Add(thisObjectPropOptionResult);

            }

            return Logger;
        }

        #endregion

        #region Activity
        public Values.AddActivity AddActivity(IConnectToDB _Connect, Values.AddActivity thisModel)
        {
            Universal_Call<Values.AddActivity> universalCall = new Universal_Call<Values.AddActivity>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public sqlTransBlocks Activity(IConnectToDB _Connect, CASTGOOP.Activity thisActivity, sqlTransBlocks Serie)
        {
            Values.AddActivity thisModel = new Values.AddActivity();

            TransactionCall<CASTGOOP.Activity, Values.AddActivity> universalCall = new TransactionCall<CASTGOOP.Activity, Values.AddActivity>();
            SQLProcedureModels.BIG_CALL params1 = universalCall.GetParams(thisActivity, thisModel);

            Serie.Series.Add(new SQLTrasaction
            {
                ProcedureName = params1.COMMANDS[0].ProcedureName,
                EntryProcedureParameters = params1.COMMANDS[0]._dbParameters
            });

            return Serie;
        }
        #endregion

        ///// <summary>
        ///// ADD Stage to the Database.
        ///// </summary>
        ///// <param name="db_platform"></param>
        ///// <param name="connauth"></param>
        ///// <param name="_stagetype"></param>
        ///// <param name="_stagename"></param>
        ///// <param name="_application_id"></param>
        ///// <param name="_containers_id"></param>
        ///// <param name="_identities_id"></param>
        ///// <returns></returns>

        //public string GridFormLink(string db_platform, string connauth, string _stagetype, string _stagename, string _application_id, string _containers_id, string _identities_id)
        //{
        //    ER_DML er_dml = new ER_DML();

        //    string result;

        //    result = er_dml.(_Connect, _stagetype, _stagename, _application_id, _containers_id, _identities_id);

        //    if (result == "")
        //    {
        //        result = "An error occurred.";
        //    }

        //    return result;
        //}

        #region Content Types
        public Values.AddContentType ADD_ENTRY_CONTENT_TYPE(IConnectToDB _Connect, Values.AddContentType thisModel)
        {
            Universal_Call<Values.AddContentType> universalCall = new Universal_Call<Values.AddContentType>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public static List<CommandResult> AddListOfContentTypes(IConnectToDB _Connect, List<Values.AddContentType> contentTypeList)
        {
            add addHelp = new add();
            List<CommandResult> Logger = new List<CommandResult>();

            for (int i = 0; i < contentTypeList.Count; i++)
            {
                contentTypeList[i] = addHelp.ADD_ENTRY_CONTENT_TYPE(_Connect, contentTypeList[i]);
                CommandResult thisTempAddSymbol = new CommandResult();
                thisTempAddSymbol.attemptedCommand = contentTypeList[i].V_ATTEMPTED_SQL;
                thisTempAddSymbol._Successful = contentTypeList[i].O_CONTENT_TYPES_ID > 0 || contentTypeList[i].O_ERR_MESS.ToLower().Contains("primary key constraint") ? true : false;
                thisTempAddSymbol._Response = thisTempAddSymbol._Successful ? "Added Content Type object " + contentTypeList[i].I_CONTENT_TYPE : "Failed to add Content Type  object " + contentTypeList[i].I_CONTENT_TYPE;

                Logger.Add(thisTempAddSymbol);
            }

            return Logger;
        }
        #endregion

        public static List<CommandResult> AddListOfRoleAndPrivs(IConnectToDB _Connect, List<Values.AddRolePrivilege> rolePriviligeList)
        {
            add addHelp = new add();
            List<CommandResult> Logger = new List<CommandResult>();

            for (int i = 0; i < rolePriviligeList.Count; i++)
            {
                rolePriviligeList[i] = addHelp.ADD_ENTRY_Priv_to_Role(_Connect, rolePriviligeList[i]);
                CommandResult thisTempAddSymbol = new CommandResult();
                thisTempAddSymbol.attemptedCommand = rolePriviligeList[i].V_ATTEMPTED_SQL;
                thisTempAddSymbol._Successful = rolePriviligeList[i].O_ROLES_PRIVILEGES_ID > 0 || rolePriviligeList[i].O_ERR_MESS.ToLower().Contains("primary key constraint") ? true : false;
                string failedMessage = "Failed to add Role Privilege object " + rolePriviligeList[i].I_PRIVILEGE_NAME;
                thisTempAddSymbol._Response = thisTempAddSymbol._Successful ? "Added  Role Privilege object " + rolePriviligeList[i].I_PRIVILEGE_NAME : failedMessage + " due to " + rolePriviligeList[i].O_ERR_MESS;

                Logger.Add(thisTempAddSymbol);
            }

            return Logger;
        }

        public static List<CommandResult> AddListOfPrivileges(IConnectToDB _Connect, List<Values.AddPrivilege> privilegeList)
        {
            add addHelp = new add();
            List<CommandResult> Logger = new List<CommandResult>();

            for (int i = 0; i < privilegeList.Count; i++)
            {
                privilegeList[i] = addHelp.ADD_ENTRY_Privilege(_Connect, privilegeList[i]);
                CommandResult thisTempAddSymbol = new CommandResult();
                thisTempAddSymbol._Successful = privilegeList[i].O_PRIVILEGES_ID > 0 || privilegeList[i].O_ERR_MESS.ToLower().Contains("primary key constraint") ? true : false;
                string failMessage = "Failed to add " + privilegeList[i].I_OBJECT_TYPE + " object " + privilegeList[i].I_PRIVILEGE_NAME;
                thisTempAddSymbol._Response = thisTempAddSymbol._Successful ? "Added  " + privilegeList[i].I_OBJECT_TYPE + " object " + privilegeList[i].I_PRIVILEGE_NAME : failMessage + " due to " + privilegeList[i].O_ERR_MESS;

                Logger.Add(thisTempAddSymbol);
            }

            return Logger;
        }

        public static List<CommandResult> AddListOfObjectLayers(IConnectToDB _Connect, List<Values.AddObjectLayer> objectLayersList)
        {
            add addHelp = new add();
            List<CommandResult> Logger = new List<CommandResult>();

            for (int i = 0; i < objectLayersList.Count; i++)
            {
                objectLayersList[i] = addHelp.ADD_ENTRY_Object_Layer(_Connect, objectLayersList[i]);
                CommandResult thisTempAddObject = new CommandResult();
                thisTempAddObject.attemptedCommand = objectLayersList[i].V_ATTEMPTED_SQL;
                thisTempAddObject._Successful = objectLayersList[i].O_OBJECT_LAYERS_ID > 0 || objectLayersList[i].O_ERR_MESS.ToLower().Contains("primary key constraint") ? true : false;
                string failMessage = "Failed to add object layer " + objectLayersList[i].I_OBJECT_LAYER;
                thisTempAddObject._Response = thisTempAddObject._Successful ? "Added Object Layer " + objectLayersList[i].I_OBJECT_LAYER : failMessage + " due to " + objectLayersList[i].I_OBJECT_LAYER;

                Logger.Add(thisTempAddObject);
            }

            return Logger;
        }

        public static List<CommandResult> AddListOfRoles(IConnectToDB _Connect, List<Values.AddRole> rolesList)
        {
            add addHelp = new add();
            List<CommandResult> Logger = new List<CommandResult>();

            for (int i = 0; i < rolesList.Count; i++)
            {
                rolesList[i] = addHelp.ADD_ENTRY_Role(_Connect, rolesList[i]);
                CommandResult thisTempAddSymbol = new CommandResult();
                thisTempAddSymbol.attemptedCommand = rolesList[i].V_ATTEMPTED_SQL;
                thisTempAddSymbol._Successful = rolesList[i].O_ROLES_ID > 0 ? true : false;
                thisTempAddSymbol._Response = thisTempAddSymbol._Successful ? "Added  " + rolesList[i].I_OBJECT_TYPE + " object " + rolesList[i].I_ROLE_NAME : "Failed to add " + rolesList[i].I_OBJECT_TYPE + " object " + rolesList[i].I_ROLE_NAME;

                Logger.Add(thisTempAddSymbol);
            }

            return Logger;
        }

        #region Containers
        public static List<CommandResult> AddListOfContainers(IConnectToDB _Connect, string CONTAINERS_ID, List<Values.AddContainer> containersList)
        {
            add addHelp = new add();
            List<CommandResult> Logger = new List<CommandResult>();

            for (int i = 0; i < containersList.Count; i++)
            {
                CommandResult addContainer = new CommandResult();
                containersList[i] = addHelp.ADD_ENTRY_Containers(_Connect, containersList[i]);
                addContainer.attemptedCommand = containersList[i].V_ATTEMPTED_SQL;
                addContainer._Successful = containersList[i].O_CONTAINERS_ID > 0 ? true : false;
                string failMessage = "Failed to Add Container " + containersList[i].I_CONTAINER_NAME;
                addContainer._Response = addContainer._Successful ? "Success Added Container " + containersList[i].I_CONTAINER_NAME : failMessage + " due to " + containersList[i].O_ERR_MESS;

                if (containersList[i].I_CONTAINER_NAME == "General")
                {
                    CONTAINERS_ID = containersList[i].O_CONTAINERS_ID.ToString();
                    addContainer._CommandName = "General Container";
                    addContainer._Return = new CommandResultReturn
                    {
                        type = CommandResultReturnTypes._int,
                        value = CONTAINERS_ID
                    };
                }

                Logger.Add(addContainer);
            }

            return Logger;
        }
        public Values.AddContainer ADD_ENTRY_Containers(IConnectToDB _Connect, Values.AddContainer thisModel)
        {
            Universal_Call<Values.AddContainer> universalCall = new Universal_Call<Values.AddContainer>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }
        #endregion

        #region Stage Containers
        public Values.AddStageContainer ADD_ENTRY_Stage_Containers(IConnectToDB _Connect, Values.AddStageContainer thisModel)
        {
            Universal_Call<Values.AddStageContainer> universalCall = new Universal_Call<Values.AddStageContainer>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }
        public sqlTransBlocks ADD_ENTRY_Stage_Containers(IConnectToDB _Connect, CASTGOOP.AddStageContainer thisStageContainer, sqlTransBlocks Serie)
        {
            Values.AddStageContainer thisModel = new Values.AddStageContainer();

            TransactionCall<CASTGOOP.AddStageContainer, Values.AddStageContainer> universalCall = new TransactionCall<CASTGOOP.AddStageContainer, Values.AddStageContainer>();
            SQLProcedureModels.BIG_CALL params1 = universalCall.GetParams(thisStageContainer, thisModel);

            Serie.Series.Add(new SQLTrasaction
            {
                ProcedureName = params1.COMMANDS[0].ProcedureName,
                EntryProcedureParameters = params1.COMMANDS[0]._dbParameters
            });

            return Serie;
        }
        #endregion

        public Values.AddObjectLayer ADD_ENTRY_Object_Layer(IConnectToDB _Connect, Values.AddObjectLayer thisModel)
        {
            Universal_Call<Values.AddObjectLayer> universalCall = new Universal_Call<Values.AddObjectLayer>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        #region Objects
        public static List<CommandResult> AddListsOfObjects(IConnectToDB _Connect, List<Values.AddObject> objectList)
        {
            add addHelp = new add();
            List<CommandResult> Logger = new List<CommandResult>();

            for (int i = 0; i < objectList.Count; i++)
            {
                objectList[i] = addHelp.ADD_ENTRY_Object(_Connect, objectList[i]);
                CommandResult thisTempAddObject = new CommandResult();
                thisTempAddObject._Successful = objectList[i].O_OBJECTS_ID > 0 || objectList[i].O_ERR_MESS.ToLower().Contains("primary key constraint") ? true : false;
                string failMessage = "Failed to add " + objectList[i].I_OBJECT_LAYER + " object " + objectList[i].I_OBJECT_TYPE;
                thisTempAddObject._Response = thisTempAddObject._Successful ? "Added " + objectList[i].I_OBJECT_LAYER + " object " + objectList[i].I_OBJECT_TYPE : failMessage + " due to " + objectList[i].O_ERR_MESS;

                Logger.Add(thisTempAddObject);
            }

            return Logger;
        }
        public Values.AddObject ADD_ENTRY_Object(IConnectToDB _Connect, Values.AddObject thisModel)
        {
            Universal_Call<Values.AddObject> universalCall = new Universal_Call<Values.AddObject>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }
        #endregion

        #region Cryptology
        public Values.AddCryptTicket ADD_ENTRY_CRYPT_TICKET(IConnectToDB _Connect, Values.AddCryptTicket thisModel)
        {
            Universal_Call<Values.AddCryptTicket> universalCall = new Universal_Call<Values.AddCryptTicket>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public Values.AddAESPair ADD_ENTRY_AES_PAIR(IConnectToDB _Connect, Values.AddAESPair thisModel)
        {
            Universal_Call<Values.AddAESPair> universalCall = new Universal_Call<Values.AddAESPair>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public string GEN_CRYPT_TICKET(IConnectToDB _Connect, long? _IdentityID)
        {

            ER_Sec er_sec = new ER_Sec();

            ER_CRYPT_PAIR _returnCryptPair = new ER_CRYPT_PAIR();

            _returnCryptPair = er_sec.Generate_Crypt_Pair(new ER_CRYPT_PAIR());

            Values.AddCryptTicket CryptTicketID = ADD_ENTRY_CRYPT_TICKET(_Connect, new Values.AddCryptTicket
            {
                I_IDENTITIES_ID = _IdentityID
            });

            Values.AddAESPair KeyPair = new Values.AddAESPair
            {
                I_AES_KEY = _returnCryptPair._KEY,
                I_AES_IV = _returnCryptPair._IV
            };

            if (CryptTicketID.O_CRYPT_TICKETS_ID > 0)
            {
                KeyPair.I_CRYPT_TICKETS_ID = CryptTicketID.O_CRYPT_TICKETS_ID;
                KeyPair = ADD_ENTRY_AES_PAIR(_Connect, KeyPair);
            }

            if (CryptTicketID.O_CRYPT_TICKETS_ID > 0 || KeyPair.O_AES_PAIR_ID > 0)
                return "Successfully Gave Crypt Ticket to Identity";
            else
                return "Could not giv Crypt Ticket to Identity";

        }
        #endregion

        public Values.AddMessage AddMessage(IConnectToDB _Connect, Values.AddMessage thisModel)
        {
            Universal_Call<Values.AddMessage> universalCall = new Universal_Call<Values.AddMessage>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public Values.AddRole ADD_ENTRY_Role(IConnectToDB _Connect, Values.AddRole thisModel)
        {

            Universal_Call<Values.AddRole> universalCall = new Universal_Call<Values.AddRole>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public Values.AddIdentityRole ADD_ENTRY_Identity_Role(IConnectToDB _Connect, Values.AddIdentityRole thisModel)
        {
            Universal_Call<Values.AddIdentityRole> universalCall = new Universal_Call<Values.AddIdentityRole>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);
            return thisModel;
        }

        public Values.AddCoreIdentity ADD_ENTRY_Cores_Identity(IConnectToDB _Connect, Values.AddCoreIdentity thisModel)
        {

            Universal_Call<Values.AddCoreIdentity> universalCall = new Universal_Call<Values.AddCoreIdentity>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);
            return thisModel;
        }

        #region Variants
        public Values.AddVariant ADD_ENTRY_Variant(IConnectToDB _Connect, Values.AddVariant thisModel)
        {
            Universal_Call<Values.AddVariant> universalCall = new Universal_Call<Values.AddVariant>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public static List<CommandResult> AddListOfVariants(IConnectToDB _Connect, List<Values.AddVariant> variantsList)
        {
            add addHelp = new add();
            List<CommandResult> Logger = new List<CommandResult>();

            for (int i = 0; i < variantsList.Count; i++)
            {
                variantsList[i] = addHelp.ADD_ENTRY_Variant(_Connect, variantsList[i]);
                CommandResult thisTempAddVariant = new CommandResult();
                thisTempAddVariant._Successful = variantsList[i].O_VARIANTS_ID > 0 ? true : false;
                thisTempAddVariant._Response = thisTempAddVariant._Successful ? "Added Variant " + variantsList[i].I_VARIANT_NAME + " color " + variantsList[i].I_OBJECT_TYPE : "Failed to add variant " + variantsList[i].I_VARIANT_NAME + " object " + variantsList[i].I_COLOR;

                Logger.Add(thisTempAddVariant);
            }

            return Logger;
        }
        #endregion

        #region Symbols
        public Values.AddSymbols ADD_ENTRY_Symbol(IConnectToDB _Connect, Values.AddSymbols thisModel)
        {
            Universal_Call<Values.AddSymbols> universalCall = new Universal_Call<Values.AddSymbols>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public static List<CommandResult> AddListOfSymbols(IConnectToDB _Connect, List<Values.AddSymbols> symbolsList)
        {
            add addHelp = new add();
            List<CommandResult> Logger = new List<CommandResult>();

            for (int i = 0; i < symbolsList.Count; i++)
            {
                symbolsList[i] = addHelp.ADD_ENTRY_Symbol(_Connect, symbolsList[i]);
                CommandResult thisTempAddSymbol = new CommandResult();
                thisTempAddSymbol.attemptedCommand = symbolsList[i].V_ATTEMPTED_SQL;
                thisTempAddSymbol._Successful = symbolsList[i].O_SYMBOLS_ID > 0 || symbolsList[i].O_ERR_MESS.ToLower().Contains("primary key constraint") ? true : false;
                thisTempAddSymbol._Response = thisTempAddSymbol._Successful ? "Added Symbol object " + symbolsList[i].I_SYMBOL_NAME : "Failed to add Symbol object " + symbolsList[i].I_SYMBOL_NAME;

                Logger.Add(thisTempAddSymbol);
            }

            return Logger;
        }
        #endregion

        public Values.AddDataType ADD_ENTRY_Value_Datatype(IConnectToDB _Connect, Values.AddDataType thisModel)
        {
            Universal_Call<Values.AddDataType> universalCall = new Universal_Call<Values.AddDataType>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public Values.AddGroup ADD_ENTRY_GROUP(IConnectToDB _Connect, Values.AddGroup thisModel)
        {
            Universal_Call<Values.AddGroup> universalCall = new Universal_Call<Values.AddGroup>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        //Add System Privileges to Roles
        public Values.AddRolePrivilege ADD_ENTRY_Priv_to_Role(IConnectToDB _Connect, Values.AddRolePrivilege thisModel)
        {
            Universal_Call<Values.AddRolePrivilege> universalCall = new Universal_Call<Values.AddRolePrivilege>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public Values.AddIdentity ADD_ENTRY_Identities(IConnectToDB _Connect, Values.AddIdentity thisModel)
        {
            Universal_Call<Values.AddIdentity> universalCall = new Universal_Call<Values.AddIdentity>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public Values.UpdateIdentity UPDATE_ENTRY_Identities(IConnectToDB _Connect, Values.UpdateIdentity thisModel)
        {
            Universal_Call<Values.UpdateIdentity> universalCall = new Universal_Call<Values.UpdateIdentity>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }
        public Values.AddProfiles ADD_ENTRY_Profiles(IConnectToDB _Connect, Values.AddProfiles thisModel)
        {
            Universal_Call<Values.AddProfiles> universalCall = new Universal_Call<Values.AddProfiles>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }
        public Values.UpdateProfiles Update_ENTRY_Profiles(IConnectToDB _Connect, Values.UpdateProfiles thisModel)
        {
            Universal_Call<Values.UpdateProfiles> universalCall = new Universal_Call<Values.UpdateProfiles>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }
        public Values.AddProfilesSecPriv ADD_ENTRY_Profiles_Sec_Priv(IConnectToDB _Connect, Values.AddProfilesSecPriv thisModel)
        {
            Universal_Call<Values.AddProfilesSecPriv> universalCall = new Universal_Call<Values.AddProfilesSecPriv>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public Values.AddProfilesDatChar ADD_ENTRY_Profiles_Dat_Char(IConnectToDB _Connect, Values.AddProfilesDatChar thisModel)
        {
            Universal_Call<Values.AddProfilesDatChar> universalCall = new Universal_Call<Values.AddProfilesDatChar>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public Values.AddProfileImages ADD_ENTRY_Profile_Images(IConnectToDB _Connect, Values.AddProfileImages thisModel)
        {
            Universal_Call<Values.AddProfileImages> universalCall = new Universal_Call<Values.AddProfileImages>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }
        public Values.UpdateProfileImages UPDATE_ENTRY_Profile_Images(IConnectToDB _Connect, Values.UpdateProfileImages thisModel)
        {
            Universal_Call<Values.UpdateProfileImages> universalCall = new Universal_Call<Values.UpdateProfileImages>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }
        public Values.AddIDPassword ADD_ENTRY_Identities_Password(IConnectToDB _Connect, Values.AddIDPassword thisModel)
        {
            ER_Sec er_sec = new ER_Sec();
            string hash = ER_Sec.ComputeHash(thisModel.V_PASSWORD, "SHA512", null);
            byte[] encryptedPassword = er_sec.EncryptStringToBytes_Aes(hash, er_sec.GetCryptPairforID(_Connect, thisModel.I_IDENTITIES_ID, new ER_CRYPT_PAIR()));
            thisModel.I_PASSWORD = encryptedPassword;

            Universal_Call<Values.AddIDPassword> universalCall = new Universal_Call<Values.AddIDPassword>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }
        public Values.UpdateIDPassword UPDATE_ENTRY_Identities_Password(IConnectToDB _Connect, Values.UpdateIDPassword thisModel)
        {
            //ER_Sec er_sec = new ER_Sec();
            //byte[] encryptedPassword = er_sec.EncryptStringToBytes_Aes(thisModel.I_PASSWORD, er_sec.GetCryptPairforID(_Connect, identitiesId, new ER_CRYPT_PAIR()));
            //thisModel.I_PASSWORD = encryptedPassword;

            Universal_Call<Values.UpdateIDPassword> universalCall = new Universal_Call<Values.UpdateIDPassword>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public Values.AddStageRelationship ADD_ENTRY_Stage_Relationships(IConnectToDB _Connect, Values.AddStageRelationship thisModel)
        {
            Universal_Call<Values.AddStageRelationship> universalCall = new Universal_Call<Values.AddStageRelationship>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public sqlTransBlocks ADD_ENTRY_Stage_Relationships(IConnectToDB _Connect, CASTGOOP.EnterStageRelationship thisStageContainer, sqlTransBlocks Serie)
        {
            Values.AddStageRelationship thisModel = new Values.AddStageRelationship();

            TransactionCall<CASTGOOP.EnterStageRelationship, Values.AddStageRelationship> universalCall = new TransactionCall<CASTGOOP.EnterStageRelationship, Values.AddStageRelationship>();
            SQLProcedureModels.BIG_CALL params1 = universalCall.GetParams(thisStageContainer, thisModel);

            Serie.Series.Add(new SQLTrasaction
            {
                ProcedureName = params1.COMMANDS[0].ProcedureName,
                EntryProcedureParameters = params1.COMMANDS[0]._dbParameters
            });

            return Serie;
        }

        public Values.AddFormRelationship ADD_ENTRY_Form_Relationships(IConnectToDB _Connect, Values.AddFormRelationship thisModel)
        {
            Universal_Call<Values.AddFormRelationship> universalCall = new Universal_Call<Values.AddFormRelationship>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public Values.AddGrid ADD_ENTRY_Grid(IConnectToDB _Connect, Values.AddGrid thisModel)
        {
            Universal_Call<Values.AddGrid> universalCall = new Universal_Call<Values.AddGrid>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public Values.AddPatch ADD_ENTRY_PATCH(IConnectToDB _Connect, Values.AddPatch thisModel)
        {
            Universal_Call<Values.AddPatch> universalCall = new Universal_Call<Values.AddPatch>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public Values.AddSession ADD_ENTRY_SESSION(IConnectToDB _Connect, Values.AddSession thisModel)
        {
            Universal_Call<Values.AddSession> universalCall = new Universal_Call<Values.AddSession>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        #region File
        public Values.AddFile ADD_ENTRY_FILE(IConnectToDB _Connect, Values.AddFile thisModel)
        {
            Universal_Call<Values.AddFile> universalCall = new Universal_Call<Values.AddFile>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public sqlTransBlocks ADD_ENTRY_FILE(IConnectToDB _Connect, CASTGOOP.EnterFile thisFile, sqlTransBlocks Serie)
        {
            Values.AddFile thisModel = new Values.AddFile();

            TransactionCall<CASTGOOP.EnterFile, Values.AddFile> universalCall = new TransactionCall<CASTGOOP.EnterFile, Values.AddFile>();
            SQLProcedureModels.BIG_CALL params1 = universalCall.GetParams(thisFile, thisModel);

            Serie.Series.Add(new SQLTrasaction
            {
                ProcedureName = params1.COMMANDS[0].ProcedureName,
                EntryProcedureParameters = params1.COMMANDS[0]._dbParameters
            });

            return Serie;
        }
        #endregion

        public Values.AddFilePoint ADD_FILE_POINT(IConnectToDB _Connect, Values.AddFilePoint thisModel)
        {
            Universal_Call<Values.AddFilePoint> universalCall = new Universal_Call<Values.AddFilePoint>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public sqlTransBlocks ADD_FILE_POINT(IConnectToDB _Connect, CASTGOOP.EnterFilePoint thisFilePoint, sqlTransBlocks Serie)
        {
            Values.AddFilePoint thisModel = new Values.AddFilePoint();

            TransactionCall<CASTGOOP.EnterFilePoint, Values.AddFilePoint> universalCall = new TransactionCall<CASTGOOP.EnterFilePoint, Values.AddFilePoint>();
            SQLProcedureModels.BIG_CALL params1 = universalCall.GetParams(thisFilePoint, thisModel);

            Serie.Series.Add(new SQLTrasaction
            {
                ProcedureName = params1.COMMANDS[0].ProcedureName,
                EntryProcedureParameters = params1.COMMANDS[0]._dbParameters
            });

            return Serie;
        }

        public Values.AddPrivilege ADD_ENTRY_Privilege(IConnectToDB _Connect, Values.AddPrivilege thisModel)
        {
            Universal_Call<Values.AddPrivilege> universalCall = new Universal_Call<Values.AddPrivilege>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        #region Identity Properties
        public Values.AddIdentityProperties ADD_ENTRY_Identity_Properties(IConnectToDB _Connect, Values.AddIdentityProperties thisModel)
        {
            Universal_Call<Values.AddIdentityProperties> universalCall = new Universal_Call<Values.AddIdentityProperties>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public sqlTransBlocks ADD_ENTRY_Identity_Properties(IConnectToDB _Connect, CASTGOOP.EnterIdentityProperty thisEnterIdentityProperty, sqlTransBlocks Serie)
        {
            Values.AddIdentityProperties thisModel = new Values.AddIdentityProperties();

            TransactionCall<CASTGOOP.EnterIdentityProperty, Values.AddIdentityProperties> universalCall = new TransactionCall<CASTGOOP.EnterIdentityProperty, Values.AddIdentityProperties>();
            SQLProcedureModels.BIG_CALL params1 = universalCall.GetParams(thisEnterIdentityProperty, thisModel);

            Serie.Series.Add(new SQLTrasaction
            {
                ProcedureName = params1.COMMANDS[0].ProcedureName,
                EntryProcedureParameters = params1.COMMANDS[0]._dbParameters
            });

            return Serie;
        }
        #endregion

        public Values.AddGroupRole ADD_ENTRY_Group_Role(IConnectToDB _Connect, Values.AddGroupRole thisModel)
        {
            Universal_Call<Values.AddGroupRole> universalCall = new Universal_Call<Values.AddGroupRole>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public Values.AddRolePrivilege ADD_ENTRY_Priv_to_Core_Role(IConnectToDB _Connect, Values.AddRolePrivilege thisModel)
        {
            Universal_Call<Values.AddRolePrivilege> universalCall = new Universal_Call<Values.AddRolePrivilege>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public Values.AddGroupMember ADD_ENTRY_GROUP_MEMBER(IConnectToDB _Connect, Values.AddGroupMember thisModel)
        {
            Universal_Call<Values.AddGroupMember> universalCall = new Universal_Call<Values.AddGroupMember>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public Values.AddToMessage SEND_MESSAGE(IConnectToDB _Connect, Values.AddToMessage thisModel)
        {
            Universal_Call<Values.AddToMessage> universalCall = new Universal_Call<Values.AddToMessage>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public Values.AddApplicationAccess ADD_APP_ACCESS(IConnectToDB _Connect, Values.AddApplicationAccess thisModel)
        {
            Universal_Call<Values.AddApplicationAccess> universalCall = new Universal_Call<Values.AddApplicationAccess>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public Values.AddFlatIcon ADD_ENTRY_FLAT_ICON(IConnectToDB _Connect, Values.AddFlatIcon thisModel)
        {
            Universal_Call<Values.AddFlatIcon> universalCall = new Universal_Call<Values.AddFlatIcon>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public Values.AddForms ADD_ENTRY_Form(IConnectToDB _Connect, Values.AddForms thisModel)
        {
            Universal_Call<Values.AddForms> universalCall = new Universal_Call<Values.AddForms>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            SecurityHelper SH = new SecurityHelper();
            ER_DML er_dml = new ER_DML();

            //string permission_Id = er_dml.ADD_PRIVILEGE_TO_OBJECT(_Connect, "Forms", thisModel.O_FORMS_ID, SH.GetPrivID(_Connect, "ADD FORM"), thisModel.I_IDENTITIES_ID);

            return thisModel;
        }

        public Values.AddFormNotes ADD_ENTRY_Form_Note(IConnectToDB _Connect, Values.AddFormNotes thisModel)
        {
            Universal_Call<Values.AddFormNotes> universalCall = new Universal_Call<Values.AddFormNotes>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        //public Values.AddProfile ADD_ENTRY_Profile(IConnectToDB _Connect, Values.AddProfile thisModel)
        //{
        //    SecurityHelper SH = new SecurityHelper();
        //    add addHelp = new add();
        //    long? profilesSecPrivId = null;
        //    Universal_Call<Values.AddProfile> _uFKKey = new Universal_Call<Values.AddProfile>();
        //    thisModel = _uFKKey.GenericInputProcedure(_Connect, thisModel);

        //    Values.AddProfilesSecPriv ProfilesSecPrivModel = null;
        //    ProfilesSecPrivModel = addHelp.ADD_ENTRY_Profiles_Sec_Priv(_Connect, new Values.AddProfilesSecPriv
        //    {
        //        I_OBJECT_TYPE = "Permission",
        //        I_PROFILES_ID = thisModel.O_PROFILES_ID,
        //        I_PRIVILEGES_ID = ER_Tools.ConvertToInt64(SH.GetPrivID(_Connect, "ADD PROFILE")),
        //        I_ENABLED = 'Y',
        //        I_IDENTITIES_ID = thisModel.I_IDENTITIES_ID
        //    });
        //    profilesSecPrivId = ProfilesSecPrivModel.O_PROFILES_SEC_PRIV_ID;

        //    //string permission_Id = er_dml.ADD_PRIVILEGE_TO_OBJECT(_Connect, "PROFILES", thisModel.O_PROFILES_ID, SH.GetPrivID(_Connect, "ADD PROFILE"), thisModel.I_IDENTITIES_ID);

        //    return thisModel;
        //}

        public Values.AddVerify ADD_ENTRY_Verify(IConnectToDB _Connect, Values.AddVerify thisModel)
        {
            Universal_Call<Values.AddVerify> _uFKKey = new Universal_Call<Values.AddVerify>();
            ER_Tools er_tools = new ER_Tools();

            thisModel.I_UUID = er_tools.RandomString(100);
            thisModel = _uFKKey.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public Values.UpdateVerify UPDATE_ENTRY_Verify(IConnectToDB _Connect, Values.UpdateVerify thisModel)
        {
            Universal_Call<Values.UpdateVerify> _uFKKey = new Universal_Call<Values.UpdateVerify>();
            ER_Tools er_tools = new ER_Tools();

            //thisModel.I_UUID = er_tools.RandomString(100);
            thisModel = _uFKKey.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public Values.AddStageRole ADD_STAGE_ROLE(IConnectToDB _Connect, Values.AddStageRole thisModel)
        {
            Universal_Call<Values.AddStageRole> universalCall = new Universal_Call<Values.AddStageRole>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public sqlTransBlocks ADD_STAGE_ROLE(IConnectToDB _Connect, CASTGOOP.AddStageRole thisTran, sqlTransBlocks Serie)
        {
            Values.AddStageRole thisModel = new Values.AddStageRole();

            TransactionCall<CASTGOOP.AddStageRole, Values.AddStageRole> universalCall = new TransactionCall<CASTGOOP.AddStageRole, Values.AddStageRole>();
            SQLProcedureModels.BIG_CALL params1 = universalCall.GetParams(thisTran, thisModel);

            Serie.Series.Add(new SQLTrasaction
            {
                ProcedureName = params1.COMMANDS[0].ProcedureName,
                EntryProcedureParameters = params1.COMMANDS[0]._dbParameters
            });

            return Serie;
        }

        public Values.AddApplicationRole ADD_APPLICATION_ROLE(IConnectToDB _Connect, Values.AddApplicationRole thisModel)
        {
            Universal_Call<Values.AddApplicationRole> universalCall = new Universal_Call<Values.AddApplicationRole>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public sqlTransBlocks ADD_APPLICATION_ROLE(IConnectToDB _Connect, CASTGOOP.AddApplicationRole thisTran, sqlTransBlocks Serie)
        {
            Values.AddApplicationRole thisModel = new Values.AddApplicationRole();

            TransactionCall<CASTGOOP.AddApplicationRole, Values.AddApplicationRole> universalCall = new TransactionCall<CASTGOOP.AddApplicationRole, Values.AddApplicationRole>();
            SQLProcedureModels.BIG_CALL params1 = universalCall.GetParams(thisTran, thisModel);

            Serie.Series.Add(new SQLTrasaction
            {
                ProcedureName = params1.COMMANDS[0].ProcedureName,
                EntryProcedureParameters = params1.COMMANDS[0]._dbParameters
            });

            return Serie;
        }

        public Values.AddObjectSetRole ADD_OBJECTSET_ROLE(IConnectToDB _Connect, Values.AddObjectSetRole thisModel)
        {
            Universal_Call<Values.AddObjectSetRole> universalCall = new Universal_Call<Values.AddObjectSetRole>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public sqlTransBlocks ADD_OBJECTSET_ROLE(IConnectToDB _Connect, CASTGOOP.AddObjectSetRole thisTran, sqlTransBlocks Serie)
        {
            Values.AddObjectSetRole thisModel = new Values.AddObjectSetRole();

            TransactionCall<CASTGOOP.AddObjectSetRole, Values.AddObjectSetRole> universalCall = new TransactionCall<CASTGOOP.AddObjectSetRole, Values.AddObjectSetRole>();
            SQLProcedureModels.BIG_CALL params1 = universalCall.GetParams(thisTran, thisModel);

            Serie.Series.Add(new SQLTrasaction
            {
                ProcedureName = params1.COMMANDS[0].ProcedureName,
                EntryProcedureParameters = params1.COMMANDS[0]._dbParameters
            });

            return Serie;
        }

        public Values.AddObjectPropSetDataFile ADD_PROP_SET_DATA_FILE(IConnectToDB _Connect, Values.AddObjectPropSetDataFile thisModel)
        {
            Universal_Call<Values.AddObjectPropSetDataFile> universalCall = new Universal_Call<Values.AddObjectPropSetDataFile>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public sqlTransBlocks ADD_PROP_SET_DATA_FILE(IConnectToDB _Connect, CASTGOOP.AddObjectPropSetDataFile thisTran, sqlTransBlocks Serie)
        {
            Values.AddObjectPropSetDataFile thisModel = new Values.AddObjectPropSetDataFile();

            TransactionCall<CASTGOOP.AddObjectPropSetDataFile, Values.AddObjectPropSetDataFile> universalCall = new TransactionCall<CASTGOOP.AddObjectPropSetDataFile, Values.AddObjectPropSetDataFile>();
            SQLProcedureModels.BIG_CALL params1 = universalCall.GetParams(thisTran, thisModel);

            Serie.Series.Add(new SQLTrasaction
            {
                ProcedureName = params1.COMMANDS[0].ProcedureName,
                EntryProcedureParameters = params1.COMMANDS[0]._dbParameters
            });

            return Serie;
        }

    }
}