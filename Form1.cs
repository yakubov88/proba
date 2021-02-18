using SmartApps.MES.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;


namespace TestScripts
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        public void TestOpScript(List<ProfileProduct> SalesItems, dsConfigurator ConfigData, dsResources DefinitionData,
            dsProductData ProfilesData)
        {
            foreach (SmartApps.MES.Data.ProfileProduct SalesItem in SalesItems)
            {
                if (!SalesItem.ProfileFullName.Contains("ИЗДЕЛИЕ"))
                {
                    //ПРЕСОВАНЕ
                    int cnt = 10;
                    ConfigData.MaterialDefinitionOperations.AddMaterialDefinitionOperationsRow(Guid.NewGuid(),
                        SalesItem.MaterialDefinition, 1, DefinitionData.OperationDefinition.FindById(1).Description, SalesItem.Quantity,
                        "КГ", "OK", "", DefinitionData.OperationDefinition.FindById(1).DefaultResourceGroup, 0,
                        Convert.ToInt32(SalesItem.Quantity * 3600 / 2000), 0, cnt);
                    //Рязане на преса
                    cnt += 10;
                    ConfigData.MaterialDefinitionOperations.AddMaterialDefinitionOperationsRow(Guid.NewGuid(), SalesItem.MaterialDefinition, 2, DefinitionData.OperationDefinition.FindById(2).Description, SalesItem.Quantity, "КГ", "OK", "", DefinitionData.OperationDefinition.FindById(2).DefaultResourceGroup, 0, 100, 0, cnt);
                    //Отгряване
                    cnt += 10;
                    int TimeToAddFurnace = 0;
                    string furnaceStatus = "OK";
                    switch (SalesItem.Options["Alloy"])
                    {
                        case "1200A":
                            switch (SalesItem.Options["Temper"])
                            {
                                case "F":
                                    TimeToAddFurnace = 0;
                                    break;
                                default:
                                    furnaceStatus = "Сплав " + SalesItem.Options["Alloy"] +
                                                    " не може да бъде в термично състояние " + SalesItem.Options["Temper"];
                                    break;
                            }

                            break;
                        case "6060":
                        case "EN AW-6063":
                            switch (SalesItem.Options["Temper"])
                            {
                                case "T4":
                                    TimeToAddFurnace = 0;
                                    break;
                                case "T5":
                                case "T64":
                                    TimeToAddFurnace = 21600;
                                    break;
                                case "WS2005 Typ U":
                                case "WS2005 Typ A":
                                case "T6":
                                case "T66":
                                case "F22":
                                case "F25":
                                case "Type U":
                                case "Тype A":

                                    TimeToAddFurnace = 43200;
                                    break;
                                default:
                                    furnaceStatus = "Сплав " + SalesItem.Options["Alloy"] +
                                                    " не може да бъде в термично състояние " + SalesItem.Options["Temper"];
                                    break;
                            }
                            break;

                        case "AlMgSi-LY":
                            switch (SalesItem.Options["Temper"])
                            {
                                case "Тype A":

                                    TimeToAddFurnace = 43200;
                                    break;
                                default:
                                    furnaceStatus = "Сплав " + SalesItem.Options["Alloy"] +
                                                    " не може да бъде в термично състояние " + SalesItem.Options["Temper"];
                                    break;
                            }

                            break;
                        case "6063":
                            switch (SalesItem.Options["Temper"])
                            {
                                case "T4":
                                    TimeToAddFurnace = 0;
                                    break;
                                case "T5":
                                case "T64":
                                    TimeToAddFurnace = 21600;
                                    break;
                                case "T6":
                                case "T66":
                                    TimeToAddFurnace = 43200;
                                    break;
                                default:
                                    furnaceStatus = "Сплав " + SalesItem.Options["Alloy"] +
                                                    " не може да бъде в термично състояние " + SalesItem.Options["Temper"];
                                    break;
                            }

                            break;
                        case "6005":
                            switch (SalesItem.Options["Temper"])
                            {
                                case "T4":
                                    TimeToAddFurnace = 7200;
                                    break;
                                case "T6":
                                    TimeToAddFurnace = 43200;
                                    break;
                                default:
                                    furnaceStatus = "Сплав " + SalesItem.Options["Alloy"] +
                                                    " не може да бъде в термично състояние " + SalesItem.Options["Temper"];
                                    break;
                            }

                            break;
                        case "6082":
                            switch (SalesItem.Options["Temper"])
                            {
                                case "T4":
                                case "F4":
                                case "F":
                                    TimeToAddFurnace = 0;
                                    break;
                                case "T5":
                                    TimeToAddFurnace = 21600;
                                    break;
                                case "T6":
                                case "F28":
                                case "F31":
                                    TimeToAddFurnace = 43200;
                                    break;
                                default:
                                    furnaceStatus = "Сплав " + SalesItem.Options["Alloy"] +
                                                    " не може да бъде в термично състояние " + SalesItem.Options["Temper"];
                                    break;
                            }

                            break;

                    }

                    if (TimeToAddFurnace > 0 || furnaceStatus != "OK")
                    {
                        if (furnaceStatus == "OK")
                        {
                            ConfigData.MaterialDefinitionOperations.AddMaterialDefinitionOperationsRow(Guid.NewGuid(),
                                SalesItem.MaterialDefinition, 3, DefinitionData.OperationDefinition.FindById(3).Description,
                                SalesItem.Quantity, "КГ", "OK", "", DefinitionData.OperationDefinition.FindById(3).DefaultResourceGroup, 0,
                                TimeToAddFurnace, 0, cnt);
                        }
                        else
                        {
                            ConfigData.MaterialDefinitionOperations.AddMaterialDefinitionOperationsRow(Guid.NewGuid(),
                                SalesItem.MaterialDefinition, 3, DefinitionData.OperationDefinition.FindById(3).Description,
                                SalesItem.Quantity, "КГ", "Error", furnaceStatus,
                                DefinitionData.OperationDefinition.FindById(3).DefaultResourceGroup, 0, TimeToAddFurnace, 0,
                                cnt);
                            SalesItem.ErrorMessage = furnaceStatus;
                        }
                    }

                    ////Пясъкоструене
                    //if (SalesItem.Options["PreAnodizingTreatment"] == "Пясъкоструене" &&
                    //    SalesItem.Options["Anodizing"] == "True")
                    //{
                    //    cnt += 10;
                    //    ConfigData.MaterialDefinitionOperations.AddMaterialDefinitionOperationsRow(Guid.NewGuid(),
                    //        SalesItem.MaterialDefinition, 6, DefinitionData.OperationDefinition.FindById(6).Description, SalesItem.Quantity,
                    //        "КГ", "OK", "", DefinitionData.OperationDefinition.FindById(6).DefaultResourceGroup, 0, 7200, 0,
                    //        cnt);
                    //}



                    //Пясъкоструене
                    if (SalesItem.Options["Surface"] == "Елоксирана" && SalesItem.Options["Color1"].Contains("SB") )
                    {
                        cnt += 10;
                        ConfigData.MaterialDefinitionOperations.AddMaterialDefinitionOperationsRow(Guid.NewGuid(),
                            SalesItem.MaterialDefinition, 6, DefinitionData.OperationDefinition.FindById(6).Description, SalesItem.Quantity,
                            "КГ", "OK", "", DefinitionData.OperationDefinition.FindById(6).DefaultResourceGroup, 0, 7200, 0,
                            cnt);
                    }



                    //Предварителна подготовка боядисване
                    if (SalesItem.Options["PowderCoating"] == "True" &&
                        SalesItem.Options["SubContractor"] != "Боядисване" &&
                        SalesItem.Options["SubContractor"] != "Пробиване и боядисване")
                    {
                        cnt += 10;
                        ConfigData.MaterialDefinitionOperations.AddMaterialDefinitionOperationsRow(Guid.NewGuid(),
                            SalesItem.MaterialDefinition, 10, DefinitionData.OperationDefinition.FindById(10).Description,
                            SalesItem.Quantity, "КГ", "OK", "", DefinitionData.OperationDefinition.FindById(10).DefaultResourceGroup, 0,
                            3600, 0, cnt);
                    }

                    //Байцване
                    if (SalesItem.Options["PreAnodizingTreatment"] == "Байцване" &&
                        SalesItem.Options["Anodizing"] == "True")
                    {
                        cnt += 10;
                        ConfigData.MaterialDefinitionOperations.AddMaterialDefinitionOperationsRow(Guid.NewGuid(),
                            SalesItem.MaterialDefinition, 11, DefinitionData.OperationDefinition.FindById(11).Description,
                            SalesItem.Quantity, "КГ", "OK", "", DefinitionData.OperationDefinition.FindById(11).DefaultResourceGroup, 0,
                            7200, 0, cnt);
                    }

                    //Четкосване
                    if (SalesItem.Options["PreAnodizingTreatment"] == "Четкосване" &&
                        SalesItem.Options["Anodizing"] == "True")
                    {
                        cnt += 10;
                        ConfigData.MaterialDefinitionOperations.AddMaterialDefinitionOperationsRow(Guid.NewGuid(),
                            SalesItem.MaterialDefinition, 12, DefinitionData.OperationDefinition.FindById(12).Description,
                            SalesItem.Quantity, "КГ", "OK", "", DefinitionData.OperationDefinition.FindById(12).DefaultResourceGroup, 0,
                            7200, 0, cnt);
                    }

                    //Подизпълнител боядисване
                    if (SalesItem.Options["SubContractor"] == "Боядисване" ||
                        SalesItem.Options["SubContractor"] == "Пробиване и боядисване")
                    {
                        cnt += 10;
                        ConfigData.MaterialDefinitionOperations.AddMaterialDefinitionOperationsRow(Guid.NewGuid(),
                            SalesItem.MaterialDefinition, 19, DefinitionData.OperationDefinition.FindById(19).Description,
                            SalesItem.Quantity, "КГ", "OK", "", DefinitionData.OperationDefinition.FindById(19).DefaultResourceGroup, 0,
                            172800, 0, cnt);
                    }

                    //Прахово боядисване
                    if (SalesItem.Options["PowderCoating"] == "True" &&
                        SalesItem.Options["SubContractor Operations"] != "Боядисване" &&
                        SalesItem.Options["SubContractor Operations"] != "Пробиване и боядисване")
                    {
                        cnt += 10;
                        var powderCoatingRow = ConfigData.MaterialDefinitionOperations.AddMaterialDefinitionOperationsRow(Guid.NewGuid(),
                            SalesItem.MaterialDefinition, 7, DefinitionData.OperationDefinition.FindById(7).Description, SalesItem.Quantity,
                            "КГ", "OK", "", DefinitionData.OperationDefinition.FindById(7).DefaultResourceGroup, 0, 7200, 0,
                            cnt);
                        if (Convert.ToInt32(SalesItem.Options["CoverThickness"]) > 100)
                        {
                            powderCoatingRow.Quantity = powderCoatingRow.Quantity * 2;
                        }
                    }

                    if (SalesItem.CustomerName == "СКАЙМЕТ ЕООД" && (SalesItem.Options["SubContractor Operations"] == "Пробиване на отвори" || SalesItem.Options["SubContractor Operations"] == "Пробиване"))
                    {
                        cnt += 10;
                        ConfigData.MaterialDefinitionOperations.AddMaterialDefinitionOperationsRow(Guid.NewGuid(),
                            SalesItem.MaterialDefinition, 20, DefinitionData.OperationDefinition.FindById(20).Description,
                            SalesItem.Quantity, "КГ", "OK", "", DefinitionData.OperationDefinition.FindById(20).DefaultResourceGroup, 0,
                            1209600, 0, cnt);
                    }

                    //Елоксация
                    if (SalesItem.Options["Anodizing"] == "True")
                    {

                        if (SalesItem.Options["SubContractor Operations"] == "Елоксация")
                        {
                            cnt += 10;
                            ConfigData.MaterialDefinitionOperations.AddMaterialDefinitionOperationsRow(Guid.NewGuid(),
                                SalesItem.MaterialDefinition, 5, DefinitionData.OperationDefinition.FindById(5).Description, SalesItem.Quantity,
                                "КГ", "OK", "", DefinitionData.OperationDefinition.FindById(5).DefaultResourceGroup, 0, 1209600,
                                0, cnt);
                        }
                        else
                        {
                            cnt += 10;
                            ConfigData.MaterialDefinitionOperations.AddMaterialDefinitionOperationsRow(Guid.NewGuid(),
                                SalesItem.MaterialDefinition, 5, DefinitionData.OperationDefinition.FindById(5).Description, SalesItem.Quantity,
                                "КГ", "OK", "", DefinitionData.OperationDefinition.FindById(5).DefaultResourceGroup, 0, 86400,
                                0, cnt);
                        }
                    }
                    

                    //Рязане циркуляр
                    if (Math.Abs(Convert.ToDecimal(SalesItem.Options["ToleranceLengthMin"])) +
                        Math.Abs(Convert.ToDecimal(SalesItem.Options["ToleranceLengthMax"])) < 5)
                    {
                        cnt += 10;
                        ConfigData.MaterialDefinitionOperations.AddMaterialDefinitionOperationsRow(Guid.NewGuid(),
                            SalesItem.MaterialDefinition, 4, DefinitionData.OperationDefinition.FindById(4).Description, SalesItem.Quantity,
                            "КГ", "OK", "", DefinitionData.OperationDefinition.FindById(4).DefaultResourceGroup, 0, 600, 0,
                            cnt);
                    }

                    //CNC
                    var countOp = ProfilesData.ProfileProduct.Count(c =>
                        c.ERPItem == SalesItem.ProfileName && c.ERPVariant == SalesItem.VariantCode);
                    if (SalesItem.Options["Drawing"] != "" || SalesItem.ProfileFullName.Contains("ИЗДЕЛИЕ"))
                    {
                        cnt += 10;
                        ConfigData.MaterialDefinitionOperations.AddMaterialDefinitionOperationsRow(Guid.NewGuid(),
                            SalesItem.MaterialDefinition, 8, DefinitionData.OperationDefinition.FindById(8).Description, SalesItem.Quantity,
                            "КГ", "OK", "", DefinitionData.OperationDefinition.FindById(8).DefaultResourceGroup, 0, 600, 0,
                            cnt);
                    }


                    //Подизпълнител отвори и рязане
                    if ((SalesItem.Options["SubContractor Operations"] == "Пробиване" || SalesItem.Options["SubContractor Operations"] == "Пробиване и боядисване"
                        || SalesItem.Options["SubContractor Operations"] == "Пробиване на отвори") && !(SalesItem.CustomerName == "СКАЙМЕТ ЕООД" && (SalesItem.Options["SubContractor Operations"] == "Пробиване на отвори" || SalesItem.Options["SubContractor Operations"] == "Пробиване")))
                    {
                        cnt += 10;
                        ConfigData.MaterialDefinitionOperations.AddMaterialDefinitionOperationsRow(Guid.NewGuid(),
                            SalesItem.MaterialDefinition, 20, DefinitionData.OperationDefinition.FindById(20).Description,
                            SalesItem.Quantity, "КГ", "OK", "", DefinitionData.OperationDefinition.FindById(20).DefaultResourceGroup, 0,
                            1209600, 0, cnt);
                    }

                    //Опаковане
                    int TimeToAdd = 3600;
                    cnt += 10;

                    switch (SalesItem.Options["Packing"])
                    {
                        case "Стара":
                            TimeToAdd = 3600;
                            break;
                        case "Нова":
                            TimeToAdd = 1800;
                            break;
                        case "Специфична":
                            TimeToAdd = 5400;
                            break;
                    }


                    ConfigData.MaterialDefinitionOperations.AddMaterialDefinitionOperationsRow(Guid.NewGuid(),
                        SalesItem.MaterialDefinition, 9, DefinitionData.OperationDefinition.FindById(9).Description, SalesItem.Quantity,
                        "КГ", "OK", "", DefinitionData.OperationDefinition.FindById(9).DefaultResourceGroup, 0, TimeToAdd,
                        0, cnt);




                    //Разместване на операции, ако имаме избор на покритие отвори
                    if (SalesItem.Options["CoverCutHoles"] != "Без покритие")
                    {
                        //Първо търсим за подготвка за боядисване и вземаме нейния пореден номер на операцията
                        if (ConfigData.MaterialDefinitionOperations.Select("OperationId=10").Count() > 0)
                        {
                            if (ConfigData.MaterialDefinitionOperations.Select("OperationId=4").Count() > 0)
                            {
                                int newIndex =
                                    Convert.ToInt32(
                                        ConfigData.MaterialDefinitionOperations.Select("OperationId=10")[0]["OpNumber"]);
                                //ConfigData.MaterialDefinitionOperations.Select("OperationId=4")[0]["OpNumber"] = Convert.ToInt32(ConfigData.MaterialDefinitionOperations.Select("OperationId=10")[0]["OpNumber"]) - 1;
                                foreach (DataRow dtRow in ConfigData.MaterialDefinitionOperations.Select(
                                    "OpNumber>=" + Convert.ToString(newIndex) + " AND OpNumber<=" +
                                    ConfigData.MaterialDefinitionOperations.Select("OperationId=10")[0]["OpNumber"]
                                        .ToString()))
                                {
                                    dtRow["OpNumber"] = Convert.ToInt32(dtRow["OpNumber"]) + 10;
                                }

                                ConfigData.MaterialDefinitionOperations.Select("OperationId=4")[0]["OpNumber"] = newIndex;
                            }
                        }
                    }
                }
                else
                {
                    //търсене в база данни с шаблони на поръчки. За търсене се използва името на поръчката в шаблона
                    if (ProfilesData.TemplateProductionOrder.Any(a => a.OrderNo == SalesItem.ProfileCode && a.ProductVariantCode == SalesItem.VariantCode))
                    {
                        var orderId = ProfilesData.TemplateProductionOrder.First(a =>
                            a.OrderNo == SalesItem.ProfileCode && a.ProductVariantCode == SalesItem.VariantCode).Id;
                        foreach (var operation in ProfilesData.TemplateProductionOrderOperation.Where(w => w.TemplateOrderId == orderId).OrderBy(o => o.OperationNo))
                        {
                            if (DefinitionData.OperationDefinition.Any(f => f.NomenclatureId == operation.OperationTypeId))
                            {
                                var operationDefinition = DefinitionData.OperationDefinition.First(f => f.NomenclatureId == operation.OperationTypeId);
                                ConfigData.MaterialDefinitionOperations.AddMaterialDefinitionOperationsRow(Guid.NewGuid(), SalesItem.MaterialDefinition, operationDefinition.Id, (!operation.IsDescriptionNull() ? operation.Description : operationDefinition.Description), SalesItem.Quantity, "КГ", "OK", "", operationDefinition.DefaultResourceGroup, (operation.IsSetupTimeNull() ? 0 : Convert.ToInt32(operation.SetupTime)), (operation.IsRunTimeNull() ? 0 : Convert.ToInt32(operation.RunTime)), (operation.IsWaitTimeNull() ? 0 : Convert.ToInt32(operation.WaitTime)), operation.OperationNo);
                            }
                            else
                            {
                                SalesItem.ErrorMessage = "Изделие с грешка в дефинирания шаблон за маршрут!";
                            }

                        }
                    }
                    else
                    {
                        if (ProfilesData.TemplateProductionOrder.Any(template => template.OrderNo == SalesItem.ProfileCode))
                        {
                            SalesItem.ErrorMessage = "Изделието е дeфинирано, но не е намерен с този код вариант!";
                        }
                        else
                        {
                            SalesItem.ErrorMessage = "Изделие без дефиниран шаблон за маршрут!";
                        }
                    }
                }
            }
        }

        public void TestResScript(List<ProfileProduct> SalesItems, dsConfigurator ConfigData,
            dsResources DefinitionData, dsProductData ProfilesData)
        {
            foreach (SmartApps.MES.Data.ProfileProduct SalesItem in SalesItems)
            {
                int TimeToAdd = 0;
                foreach (dsConfigurator.MaterialDefinitionOperationsRow operationRow in SalesItem.MaterialDefinition.GetMaterialDefinitionOperationsRows())
                {
                    switch (operationRow.OperationId)
                    {
                        //Пресоване
                        case 1:
                            var innerRows = new List<DataRow>();
                            string reasons = System.String.Empty;
                            foreach (dsConfigurator.MaterialDefinitionOperationResourcesRow possibleResRow in operationRow.GetMaterialDefinitionOperationResourcesRows())
                            {
                                //Проверяваме дали дължината на профила е по-голяма от максималната дължина на рязане за всяка преса

                                //ConfigData.ResourcesAttributes.Select("MatDefOpResId = '" + Convert.ToString(possibleResRow["Id"]) +  "' AND Code = 'Lmax'").Count() > 0
                                if (ConfigData.ResourcesAttributes.Any(maxPress => maxPress.MatDefOpResId == possibleResRow.Id && maxPress.Code == "Lmax")
                                        &&
                                     Convert.ToInt32(ConfigData.ResourcesAttributes.First(maxPress => maxPress.MatDefOpResId == possibleResRow.Id && maxPress.Code == "Lmax").Value)
                                     < SalesItem.Length)
                                {
                                    innerRows.Add(possibleResRow);
                                    reasons += Convert.ToString(
                                                   DefinitionData.Resources.Select(
                                                       "ResourcesId = " +
                                                       Convert.ToString(possibleResRow["ResourceId"]))[0]["ID"]) +
                                               " - дължина на профила над максималната\r\n";
                                }

                                //Проверяваме дали сплавта е 6082 и изтриваме всички преси освен преса 2000т
                                if (SalesItem.Options["Alloy"] == "6082" && possibleResRow.ResourceId != 10 && possibleResRow.ResourceId != 29)
                                {
                                    innerRows.Add(possibleResRow);
                                    //reasons += Convert.ToString(DefinitionData.Resources.Select("ResourcesId = " + Convert.ToString(possibleResRow["ResourceId"]))[0]["ID"]) + " - сплав 6082 се прави само на преса 2000т\r\n";
                                }

                                //Проверяваме дали сплавта е 1200/A и изтриваме всички преси освен преса 1300т
                                if (SalesItem.Options["Alloy"] == "1200A" &&
                                    Convert.ToInt32(possibleResRow["ResourceId"]) != 8)
                                {
                                    innerRows.Add(possibleResRow);
                                    // reasons += Convert.ToString(DefinitionData.Resources.Select("ResourcesId = " + Convert.ToString(possibleResRow["ResourceId"]))[0]["ID"]) + " - сплав 1200/A се прави само на преса 1300т\r\n";
                                }

                                //Проверяваме дали сплавта е 1200/A и изтриваме всички преси освен преса 1300т
                                if (SalesItem.Options["Alloy"] != "6060" && SalesItem.Options["Alloy"] != "6063" &&
                                    Convert.ToInt32(possibleResRow["ResourceId"]) == 7)
                                {
                                    innerRows.Add(possibleResRow);
                                    //reasons += Convert.ToString(DefinitionData.Resources.Select("ResourcesId = " + Convert.ToString(possibleResRow["ResourceId"]))[0]["ID"]) + " - сплав 1200/A се прави само на преса 1300т\r\n";
                                }

                                //..... и т.н.
                            }

                            string AlloyToSearch = SalesItem.Options["Alloy"];
                            if (AlloyToSearch == "6063" || AlloyToSearch == "AlMgSi-CH")

                            {
                                AlloyToSearch = "6060";
                            }
                            else if (AlloyToSearch == "1200А")
                            {
                                AlloyToSearch = "1200A";
                            }

                            if (ProfilesData.ProfilesByPress.Any(a => a.ProfileId == SalesItem.IDinTechDB && a.AlloyFamily == AlloyToSearch))
                            {
                                //ако имаме данни за Профил-Сплав, то маркираме за премахване пресите,  коите не са в базата данни
                                foreach (DataRow possibleResRow in operationRow.GetChildRows(
                                    "MaterialDefinitionOperations_MaterialDefinitionOperationResources"))
                                {
                                    if (ProfilesData.ProfilesByPress
                                            .Select("ProfileId = " + Convert.ToString(SalesItem.IDinTechDB) +
                                                    " AND AlloyFamily = '" + AlloyToSearch + "' AND PressID = " +
                                                    Convert.ToString(possibleResRow["ResourceId"])).Count() > 0)
                                    {
                                        //Подреждаме пресите спрямо приоритета в базата данни
                                        possibleResRow["Priority"] = ProfilesData.ProfilesByPress.Select(
                                            "ProfileId = " + Convert.ToString(SalesItem.IDinTechDB) +
                                            " AND AlloyFamily = '" + AlloyToSearch + "' AND PressID = " +
                                            Convert.ToString(possibleResRow["ResourceId"]))[0]["Priority"];
                                    }
                                    else
                                    {
                                        innerRows.Add(possibleResRow);
                                        reasons += Convert.ToString(
                                                       DefinitionData.Resources.Select(
                                                           "ResourcesId = " +
                                                           Convert.ToString(possibleResRow["ResourceId"]))[0]["ID"]) +
                                                   " - неналичие в технологичната база данни\r\n";
                                    }
                                }
                            }
                            else
                            //ако не е в базата данни, то не премахваме нищо, но показваме предупреждение
                            {
                                operationRow["Status"] = "Warning";
                                //reasons = "Липсват данни в технологичната база данни за комбинацията Профил-Сплав-Преса\r\n" + reasons;
                            }

                            foreach (DataRow row in innerRows)
                            {
                                //var rows = ConfigData.ResourcesAttributes.Select("MatDefOpResId=" + Convert.ToString(row["Id"]));
                                //foreach (var rw in rows)
                                //{rw.Delete();}
                                row.Delete();
                            }

                            //Показваме грешка, ако няма налични ресурси за изпълнението на пресоването
                            if (operationRow
                                    .GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources")
                                    .Count() == 0)
                            {
                                operationRow["Status"] = "Error";
                                reasons = "Липсват преси, на които може да бъде изпълнено заданието\r\n" + reasons;
                                SalesItem.ErrorMessage = reasons;
                            }

                            //MessageBox.Show(reasons);
                            operationRow["StatusMessage"] = reasons;
                            break;
                        //Рязане на преса
                        case 2:
                            TimeToAdd = 1800;
                            break;
                        //Отгряване (Пещ)
                        case 3:
                            TimeToAdd = 5400;
                            break;
                        //Пясъкоструене
                        case 6:
                            TimeToAdd = 5400;
                            break;
                        //Предварителна обр. пр. боядисване
                        case 10:
                            TimeToAdd = 5400;
                            break;
                        //Байцване
                        case 11:
                            TimeToAdd = 5400;
                            break;
                        //Четкосване
                        case 12:
                            TimeToAdd = 5400;
                            break;
                        //Подизпълнител боядисване
                        case 19:
                            TimeToAdd = 5400;
                            break;
                        //Елоксация
                        case 5:
                            var rowsToDeleteAnnodizing = new List<DataRow>();
                            foreach (DataRow possibleResRow in operationRow.GetChildRows(
                                "MaterialDefinitionOperations_MaterialDefinitionOperationResources"))
                            {
                                //Проверяваме дали дължината на профила е по-голяма от макcималната дължина на рязане за всяка преса
                                if (ConfigData.ResourcesAttributes
                                        .Select("MatDefOpResId = '" + Convert.ToString(possibleResRow["Id"]) +
                                                "' AND Code = 'Lmax(an)'").Count() > 0 &&
                                    Convert.ToInt32(ConfigData.ResourcesAttributes.Select(
                                        "MatDefOpResId = '" + Convert.ToString(possibleResRow["Id"]) +
                                        "' AND Code = 'Lmax(an)'")[0]["Value"]) <
                                    SalesItem.Length)
                                {
                                    var rows = ConfigData.ResourcesAttributes.Select(
                                        "MatDefOpResId='" + Convert.ToString(possibleResRow["Id"]) + "'");
                                    foreach (var row in rows)
                                        row.Delete();
                                    rowsToDeleteAnnodizing.Add(possibleResRow);
                                }

                                if (SalesItem.Options["SubContractor Operations"] == "Елоксация")
                                {
                                    possibleResRow["Priority"] = 1;
                                    if (operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").Any(w => Convert.ToInt32((object)w["ResourceId"]) == 13))
                                    {
                                        operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").First(w => Convert.ToInt32((object)w["ResourceId"]) == 13).Delete();
                                    }


                                }

                                //Показваме грешка, ако няма налични ресурси за изпълнението на пресоването
                                // if (SalesItem.Options["CoverCutHoles"]) == "Само срязове") 
                                //{
                                //	operationRow["Status"] = "Error";
                                //	operationRow["StatusMessage"] = "Избрано е покритие на срязовете, което е невъзможно при елоксация";
                                //} 
                                //Проверяваме дали сплавта е 6082 и изтриваме всички преси освен преса 2000т
                                //if (SalesItem.Options["Alloy"] == "6082" && Convert.ToInt32(possibleResRow["ResourceId"]) != 9) {rowsToDeleteAnnodizing.Add(possibleResRow);}
                                //..... и т.н.
                            }

                            foreach (DataRow row in rowsToDeleteAnnodizing)
                            {
                                row.Delete();
                            }

                          
                            //if (ProfilesData.Profiles.FindByProfileId(SalesItem.IDinTechDB) != null)
                            //    {
                            //    if (SalesItem.Length > 5000 && ProfilesData.Profiles.FindByProfileId(SalesItem.IDinTechDB).Section > 75 && SalesItem.Options["TrailsFromTouches"] != "на 50 мм от двата края и в средата" && SalesItem.Options["TrailsFromTouches"] != "на 40 мм от двата края и в средата")
                            //        {
                            //        operationRow["Status"] = "Warning";
                            //        operationRow["StatusMessage"] = "Не е описано, че е възможно да има следи от контакти, а профила е над 5000 и със сечение >75";
                            //        }

                            //    }
                            //else
                            //    {
                            //    operationRow["Status"] = "Warning";
                            //    operationRow["StatusMessage"] = "Липсват данни за сечение в технологичната база данни";
                            //    }
                            //Показваме грешка, ако няма налични ресурси за изпълнението на пресоването
                            if (operationRow
                                    .GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources")
                                    .Count() == 0)
                            {
                                operationRow["Status"] = "Error";
                                operationRow["StatusMessage"] =
                                    "Дължината на профила е по-голяма от максималната възможна за елоксация";
                                SalesItem.ErrorMessage = operationRow["StatusMessage"].ToString();
                            }

                            break;
                        //Прахово боядисване
                        case 7:
                            string reasonsPC = System.String.Empty;
                            var rowsToDeletePC = new List<DataRow>();
                            var lengthFinalProduct = Convert.ToInt32(SalesItem.Options["Length"]);
                            decimal maxLengthPowderAnodizing;
                            maxLengthPowderAnodizing =
                                                       (SalesItem.Options["Anodizing"] == "True" && ProfilesData.Profiles
                                                            .FindByProfileId(SalesItem.IDinTechDB).IsLengthMaxAnodizingNull()
                                                           ? Convert.ToDecimal(DefinitionData.ResourcesAttributes.FindById(28)
                                                               .Value)
                                                           : (SalesItem.Options["PowderCoating"] == "True" &&
                                                              ProfilesData.Profiles.FindByProfileId(SalesItem.IDinTechDB)
                                                                  .IsLengthMaxPowderNull()
                                                               ? Convert.ToDecimal(DefinitionData.ResourcesAttributes
                                                                   .FindById(28).Value)
                                                               : Convert.ToDecimal(ProfilesData.Profiles
                                                                   .FindByProfileId(SalesItem.IDinTechDB).LengthMaxPowder)));
                            if (maxLengthPowderAnodizing == 0) { maxLengthPowderAnodizing = 6500; }
                            foreach (dsConfigurator.MaterialDefinitionOperationResourcesRow possibleResRow in operationRow.GetMaterialDefinitionOperationResourcesRows())
                            {
                                if (possibleResRow.RowState != DataRowState.Detached)
                                {
                                    //Проверяваме дали дължината на профила е по-голяма от макcималната дължина на рязане за всяка преса
                                    if (ConfigData.ResourcesAttributes
                                            .Select("MatDefOpResId = '" + Convert.ToString(possibleResRow["Id"]) +
                                                    "' AND Code = 'Lpc'").Count() > 0 &&
                                        Convert.ToInt32(ConfigData.ResourcesAttributes.Select(
                                            "MatDefOpResId = '" + Convert.ToString(possibleResRow["Id"]) +
                                            "' AND Code = 'Lpc'")[0]["Value"]) <
                                        SalesItem.Length)
                                    {
                                        reasonsPC +=
                                            Convert.ToString(DefinitionData.Resources.Select(
                                                "ResourcesId = " +
                                                Convert.ToString(possibleResRow["ResourceId"]))[0]["ID"]) +
                                            " - дължина на профила над максималната\r\n";
                                        var rows = ConfigData.ResourcesAttributes.Select(
                                            "MatDefOpResId='" + Convert.ToString(possibleResRow["Id"]) + "'");
                                        foreach (var row in rows)
                                            row.Delete();
                                        rowsToDeletePC.Add(possibleResRow);
                                    }

                                    if (lengthFinalProduct > maxLengthPowderAnodizing)
                                    {
                                        operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").First(w => Convert.ToInt32((object)w["ResourceId"]) == 15).Delete();
                                    }

                                    //Проверяваме дали сплавта е 6082 и изтриваме всички преси освен преса 2000т
                                    //if (SalesItem.Options["Alloy"] == "6082" && Convert.ToInt32(possibleResRow["ResourceId"]) != 9) {rowsToDeletePC.Add(possibleResRow);}
                                    //..... и т.н.
                                }

                            }
                            if (SalesItem.Options["SubContractor Operations"] == "Елоксация")
                            {
                                if (operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").Any(w => Convert.ToInt32((object)w["ResourceId"]) == 32))
                                {
                                    operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").First(w => Convert.ToInt32((object)w["ResourceId"]) == 32).Delete();

                                }
                                else
                                {

                                }

                            }
                            else
                            {
                                if (Convert.ToInt32(SalesItem.Options["LengthSubcontractor"]) == 0)
                                {
                                    operationRow["Status"] = "Error";
                                    SalesItem.ErrorMessage = "Избрана е операция подизпълнител прахово, но не е указана дължина за подизпълнител!!!";

                                  //   RaiseErrorIfNoOtherResources(SalesItem, null, "Избрана е операция подизпълнител прахово, но не е указана дължина за подизпълнител!!!");
                                }
                                else
                                {
                                    if (operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").Any(w => Convert.ToInt32((object)w["ResourceId"]) == 15))
                                    {
                                        operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").First(w => Convert.ToInt32((object)w["ResourceId"]) == 15).Delete();
                                    }

                                }
                            }

                            foreach (DataRow row in rowsToDeletePC)
                            {
                                row.Delete();
                            }

                            //Показваме грешка, ако няма налични ресурси за изпълнението на пресоването
                            if (operationRow
                                    .GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources")
                                    .Count() == 0)
                            {
                                operationRow["Status"] = "Error";
                                reasonsPC =
                                    "Дължината на профила е по-голяма от максималната възможна за прахово боядисване\r\n" +
                                    reasonsPC;
                                SalesItem.ErrorMessage = reasonsPC;
                            }

                            operationRow["StatusMessage"] = reasonsPC;
                            int priority = 1;
                            foreach (dsConfigurator.MaterialDefinitionOperationResourcesRow possibleResRow in operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources"))
                            {
                                if (possibleResRow.ResourceId == 32)
                                {
                                    possibleResRow.Duration = 1209600;
                                }//3 tona на денонощие
                                else
                                {
                                    possibleResRow.Duration = Convert.ToInt32(Convert.ToDecimal(SalesItem.Quantity) / 125 * 3600);
                                }
                                possibleResRow.Priority = priority;
                                priority++;
                            }
                            break;
                        //Рязане (циркуляр)
                        case 4:
                            string reasonsSAW = System.String.Empty;
                            var rowsToDeleteSAW = new List<DataRow>();
                            foreach (DataRow possibleResRow in operationRow.GetChildRows(
                                "MaterialDefinitionOperations_MaterialDefinitionOperationResources"))
                            {
                                //Проверяваме дали дължината на профила е по-голяма от макcималната дължина на рязане за всяка преса
                                if (ConfigData.ResourcesAttributes
                                        .Select("MatDefOpResId = '" + Convert.ToString(possibleResRow["Id"]) +
                                                "' AND Code = 'Lmax_saw'").Count() > 0 &&
                                    Convert.ToInt32(ConfigData.ResourcesAttributes.Select(
                                        "MatDefOpResId = '" + Convert.ToString(possibleResRow["Id"]) +
                                        "' AND Code = 'Lmax_saw'")[0]["Value"]) <
                                    SalesItem.Length)
                                {
                                    reasonsSAW +=
                                        Convert.ToString(DefinitionData.Resources.Select(
                                            "ResourcesId = " +
                                            Convert.ToString(possibleResRow["ResourceId"]))[0]["ID"]) +
                                        " - дължина на профила над максималната\r\n";
                                    var rows = ConfigData.ResourcesAttributes.Select(
                                        "MatDefOpResId='" + Convert.ToString(possibleResRow["Id"]) + "'");
                                    foreach (var row in rows)
                                        row.Delete();
                                    rowsToDeleteSAW.Add(possibleResRow);
                                }

                                //Проверяваме дали дължината на профила е по-голяма от макcималната дължина на рязане за всяка преса
                                if (ConfigData.ResourcesAttributes
                                        .Select("MatDefOpResId = '" + Convert.ToString(possibleResRow["Id"]) +
                                                "' AND Code = 'Lmin_saw'").Count() > 0 &&
                                    Convert.ToInt32(ConfigData.ResourcesAttributes.Select(
                                        "MatDefOpResId = '" + Convert.ToString(possibleResRow["Id"]) +
                                        "' AND Code = 'Lmin_saw'")[0]["Value"]) >
                                    SalesItem.Length)
                                {
                                    reasonsSAW +=
                                        Convert.ToString(DefinitionData.Resources.Select(
                                            "ResourcesId = " +
                                            Convert.ToString(possibleResRow["ResourceId"]))[0]["ID"]) +
                                        " - дължина на профила под минималната\r\n";
                                    var rows = ConfigData.ResourcesAttributes.Select(
                                        "MatDefOpResId='" + Convert.ToString(possibleResRow["Id"]) + "'");
                                    foreach (var row in rows)
                                        row.Delete();
                                    rowsToDeleteSAW.Add(possibleResRow);
                                }
                            }

                            foreach (DataRow row in rowsToDeleteSAW)
                            {
                                row.Delete();
                            }

                            //Показваме грешка, ако няма налични ресурси за изпълнението на пресоването
                            if (!operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").Any())
                            {
                                operationRow["Status"] = "Error";
                                reasonsSAW =
                                    "Дължината на профила е по-голяма от максималната възможна за прахово боядисване\r\n" +
                                    reasonsSAW;
                                SalesItem.ErrorMessage = reasonsSAW;
                            }

                            operationRow["StatusMessage"] = reasonsSAW;
                            foreach (dsConfigurator.MaterialDefinitionOperationResourcesRow possibleResRow in operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources"))
                            {
                                possibleResRow.Duration = Convert.ToInt32(Convert.ToDecimal(SalesItem.Quantity) / 160 * 3600);
                            }
                            break;
                        //Пробиване и рязане
                        case 8:
                            var techDataCounter = ProfilesData.ProfileProduct.Count(c=> c.ERPItem== SalesItem.ProfileCode && c.ERPVariant ==SalesItem.VariantCode);
                            var cncOpCounter = SalesItem.MaterialDefinition.GetMaterialDefinitionOperationsRows().Count(w => w.OpNumber < operationRow.OpNumber && w.OperationId == 8);


                            if (cncOpCounter +1 <= techDataCounter)
                            {
                                var techData = ProfilesData.ProfileProduct.Where(c=> c.ERPItem== SalesItem.ProfileCode && c.ERPVariant ==SalesItem.VariantCode).ToList().OrderBy(o=>o.OpNo).ToList()[cncOpCounter];
                                int numberOfPiecesApprox = Convert.ToInt32(SalesItem.Quantity / (techData.WeightPerPiece / 1000));
                                if (!techData.IsCNC1Null() && techData.CNC1 > 0)
                                {
                                    if (operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").Any(w => Convert.ToInt32((object)w["ResourceId"]) == 16)) operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").First(w => Convert.ToInt32((object)w["ResourceId"]) == 16)["Duration"] = techData.MinutesPerPiece * 60 * numberOfPiecesApprox;
                                    if (operationRow.GetMaterialDefinitionOperationResourcesRows().Any(w => w.ResourceId == 16)) operationRow.GetMaterialDefinitionOperationResourcesRows().First(w => w.ResourceId == 16).Priority = techData.CNC1;
                                }
                                else
                                {
                                    if (operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").Any(w => Convert.ToInt32((object)w["ResourceId"]) == 16)) operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").First(w => Convert.ToInt32((object)w["ResourceId"]) == 16).Delete();
                                }
                                if (!techData.IsCNC2Null() && techData.CNC2 > 0)
                                {
                                    if (operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").Any(w => Convert.ToInt32((object)w["ResourceId"]) == 33)) operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").First(w => Convert.ToInt32((object)w["ResourceId"]) == 33)["Duration"] = techData.MinutesPerPiece * 60 * numberOfPiecesApprox;
                                    if (operationRow.GetMaterialDefinitionOperationResourcesRows().Any(w => w.ResourceId == 33)) operationRow.GetMaterialDefinitionOperationResourcesRows().First(w => w.ResourceId == 33).Priority = techData.CNC2;
                                }
                                else
                                {
                                    if (operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").Any(w => Convert.ToInt32((object)w["ResourceId"]) == 33)) operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").First(w => Convert.ToInt32((object)w["ResourceId"]) == 33).Delete();
                                }
                                if (!techData.IsSubContractor1Null() && techData.SubContractor1 > 0)
                                {
                                    if (operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").Any(w => Convert.ToInt32((object)w["ResourceId"]) == 31)) operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").First(w => Convert.ToInt32((object)w["ResourceId"]) == 31)["Duration"] = techData.MinutesPerPiece * 60 * numberOfPiecesApprox;
                                    if (operationRow.GetMaterialDefinitionOperationResourcesRows().Any(w => w.ResourceId == 31)) operationRow.GetMaterialDefinitionOperationResourcesRows().First(w => w.ResourceId == 31).Priority = techData.SubContractor1;
                                }
                                else
                                {
                                    if (operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").Any(w => Convert.ToInt32((object)w["ResourceId"]) == 31)) operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").First(w => Convert.ToInt32((object)w["ResourceId"]) == 31).Delete();
                                }
                                if (!techData.IsPunching1Null() && techData.Punching1 > 0)
                                {
                                    if (operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").Any(w => Convert.ToInt32((object)w["ResourceId"]) == 34)) operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").First(w => Convert.ToInt32((object)w["ResourceId"]) == 34)["Duration"] = techData.MinutesPerPiece * 60 * numberOfPiecesApprox;
                                    if (operationRow.GetMaterialDefinitionOperationResourcesRows().Any(w => w.ResourceId == 34)) operationRow.GetMaterialDefinitionOperationResourcesRows().First(w => w.ResourceId == 34).Priority = techData.Punching1;
                                }
                                else
                                {
                                    if (operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").Any(w => Convert.ToInt32((object)w["ResourceId"]) == 34)) operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").First(w => Convert.ToInt32((object)w["ResourceId"]) == 34).Delete();
                                }
                                if (!techData.IsPunching2Null() && techData.Punching2 > 0)
                                {
                                    if (operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").Any(w => Convert.ToInt32((object)w["ResourceId"]) == 35)) operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").First(w => Convert.ToInt32((object)w["ResourceId"]) == 35)["Duration"] = techData.MinutesPerPiece * 60 * numberOfPiecesApprox;
                                    if (operationRow.GetMaterialDefinitionOperationResourcesRows().Any(w => w.ResourceId == 35)) operationRow.GetMaterialDefinitionOperationResourcesRows().First(w => w.ResourceId == 35).Priority = techData.Punching2;
                                }
                                else
                                {
                                    if (operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").Any(w => Convert.ToInt32((object)w["ResourceId"]) == 35)) operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").First(w => Convert.ToInt32((object)w["ResourceId"]) == 35).Delete();
                                }
                                if (!techData.IsGarda3Null() && techData.Garda3 > 0)
                                {
                                    if (operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").Any(w => Convert.ToInt32((object)w["ResourceId"]) == 36)) operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").First(w => Convert.ToInt32((object)w["ResourceId"]) == 36)["Duration"] = techData.MinutesPerPiece * 60 * numberOfPiecesApprox;
                                    if (operationRow.GetMaterialDefinitionOperationResourcesRows().Any(w => w.ResourceId == 36)) operationRow.GetMaterialDefinitionOperationResourcesRows().First(w => w.ResourceId == 36).Priority = techData.Garda3;
                                }
                                else
                                {
                                    if (operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").Any(w => Convert.ToInt32((object)w["ResourceId"]) == 36)) operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources").First(w => Convert.ToInt32((object)w["ResourceId"]) == 36).Delete();
                                }
                            }
                            else
                            {
                                operationRow["Status"] = "Error";
                                operationRow["StatusMessage"] = $"Изделието не съществува в технологичната база!";
                                SalesItem.ErrorMessage += operationRow["StatusMessage"];
                            }
                            break;
                        //Подизпълнител отвори и рязане
                        case 20:
                            TimeToAdd = 5400;
                            break;
                        //Опаковане
                        case 9:
                            TimeToAdd = 5400;
                            foreach (dsConfigurator.MaterialDefinitionOperationResourcesRow possibleResRow in
                                operationRow.GetChildRows(
                                    "MaterialDefinitionOperations_MaterialDefinitionOperationResources"))
                            {
                                possibleResRow.Duration =
                                    Convert.ToInt32(Convert.ToDecimal(SalesItem.Quantity) / 3000 * 3600);
                            }

                            break;
                    }
                }
            }
        }

        public void TestOpAttribScript(List<ProfileProduct> salesItems, dsConfigurator configData,
            dsResources definitionData, dsProductData profilesData)
        {
            int Lprkr = 0;
            foreach (SmartApps.MES.Data.ProfileProduct salesItem in salesItems.Where(w => w.IDinTechDB == 0))
            {
                salesItem.ErrorMessage = "Профилът не е създаден в технологичната база данни";
            }

            foreach (SmartApps.MES.Data.ProfileProduct salesItem in salesItems.Where(w => w.IDinTechDB > 0))
            {
                foreach (SmartApps.MES.Data.dsConfigurator.MaterialDefinitionOperationsRow operationRow in salesItem
                    .MaterialDefinition.GetMaterialDefinitionOperationsRows())
                {
                    int TimeToAdd = 0;
                   
                    foreach (SmartApps.MES.Data.dsConfigurator.MDOAByRRow attributeRow in operationRow.GetMDOAByRRows().Where(w => w.RowState != DataRowState.Detached))
                    {
                        if (attributeRow.MaterialDefinitionOperationResourcesRow != null
                        ) //Проверка дали реда не е изтрит междувременно
                        {
                            int CurrResId = attributeRow.MaterialDefinitionOperationResourcesRow.ResourceId;
                            string AlloyToSearch = salesItem.Options["Alloy"];
                            if (AlloyToSearch == "6063" || AlloyToSearch == "AlMgSi-CH")
                            {
                                AlloyToSearch = "6060";
                            }
                            else if (AlloyToSearch == "1200А")
                            {
                                AlloyToSearch = "1200A";
                            }

                            int Spr = 0;
                            if (attributeRow.Code == "Lpress" &&
                                profilesData.ProfilesByPress.Any(a => a.ProfileId == salesItem.IDinTechDB && a.AlloyFamily == AlloyToSearch && a.PressID == CurrResId)
                                && profilesData.ProfilesByPress.First(a => a.ProfileId == salesItem.IDinTechDB && a.AlloyFamily == AlloyToSearch && a.PressID == CurrResId).Speed > 0)
                            {
                                Spr = profilesData.ProfilesByPress.First(a => a.ProfileId == salesItem.IDinTechDB && a.AlloyFamily == AlloyToSearch && a.PressID == CurrResId).Speed;
                                if (configData.MDOAByR.Any(f => f.Code == "Sпр" && f.OperationId == attributeRow.OperationId && f.MDORId == attributeRow.MDORId))
                                    configData.MDOAByR.First(f => f.Code == "Sпр" && f.OperationId == attributeRow.OperationId && f.MDORId == attributeRow.MDORId).Value = Spr.ToString();
                            }

                            switch (Convert.ToInt32(attributeRow.AttributeId))
                            {
                                //Lпр кр - дължина на пресоване крайна
                                case 1:
                                    if (!profilesData.ProfileProduct.Any(a => a.ERPItem == salesItem.ProfileCode && a.ERPVariant == salesItem.VariantCode))
                                    //Не е изделие, влизаме в калкулации
                                    {
                                        decimal maxLengthPowderAnodizing;
                                        attributeRow.Value = salesItem.Options["Length"];
                                        if (Convert.ToInt32(salesItem.Options["LengthSubcontractor"]) == 0)
                                        {
                                            if (salesItem.Options["LengthFinalProduct"] == "" ||
                                                salesItem.Options["LengthFinalProduct"] == "0")
                                            {
                                                if (salesItem.Options["PowderCoating"] == "True" ||
                                                    salesItem.Options["Anodizing"] == "True")
                                                {

                                                    //Търсим от дефинициите на ресурсите максималната дължина за елоксация. !!! ако липсва този ред, то кодът в този му вид ще гърми
                                                    maxLengthPowderAnodizing =
                                                       (salesItem.Options["Anodizing"] == "True" && profilesData.Profiles
                                                            .FindByProfileId(salesItem.IDinTechDB).IsLengthMaxAnodizingNull()
                                                           ? Convert.ToDecimal(definitionData.ResourcesAttributes.FindById(28)
                                                               .Value)
                                                           : (salesItem.Options["PowderCoating"] == "True" &&
                                                              profilesData.Profiles.FindByProfileId(salesItem.IDinTechDB)
                                                                  .IsLengthMaxPowderNull()
                                                               ? Convert.ToDecimal(definitionData.ResourcesAttributes
                                                                   .FindById(28).Value)
                                                               : Convert.ToDecimal(profilesData.Profiles
                                                                   .FindByProfileId(salesItem.IDinTechDB).LengthMaxPowder)));
                                                    if (maxLengthPowderAnodizing == 0) { maxLengthPowderAnodizing = 6500; }

                                                    if (salesItem.Options["TrailsFromTouches"] == "Без следи от контакти!" ||
                                                        salesItem.Options["TrailsFromTouches"] == "Само в средата." ||
                                                        Math.Truncate(Convert.ToDecimal(
                                                            (maxLengthPowderAnodizing - 10) /
                                                            Convert.ToDecimal(attributeRow.Value))) >= 3 ||
                                                        ((Math.Abs(Convert.ToDecimal(salesItem.Options["ToleranceLengthMin"])) +
                                                          Math.Abs(Convert.ToDecimal(salesItem.Options["ToleranceLengthMax"]))
                                                            ) < 5))
                                                    {
                                                        /* int n = 0;
                                                        n = Convert.ToInt32(Math.Truncate(Convert.ToDecimal((Convert.ToInt32(DefinitionData.ResourcesAttributes.Select("Id = 28")[0]["Value"]) - 235) / (Convert.ToDecimal(attributeRow.Value) + 5))));
                                                        if (n != 1)
                                                            {
                                                            attributeRow.Value = n * SalesItem.Length + (n + 1) * 5 + 180;
                                                            if (Convert.ToInt32(attributeRow.Value) > 5000)
                                                                {
                                                                attributeRow.Value = Convert.ToInt32(attributeRow.Value) + 50;
                                                                }
                                                            //MessageBox.Show(Convert.ToString(n));
                                                            ConfigData.MDOAByR.Select("Code = 'Nпр'")[0]["Value"] = n;
                                                            }
                                                        else
                                                            {
                                                            attributeRow.GetParentRow("FK_MaterialDefinitionOperationAttributes_ToMaterialDefinitionOperations")["Status"] = "Error";
                                                            attributeRow.GetParentRow("FK_MaterialDefinitionOperationAttributes_ToMaterialDefinitionOperations")["StatusMessage"] = "Искане за профил без следи, но без възможност за повече от един";
                                                            } */
                                                        if (Convert.ToDouble(attributeRow.Value) > Convert.ToInt32(definitionData.ResourcesAttributes.First(f => f.Id == 1004).Value))
                                                        {
                                                            int n = 0;
                                                            int Lsrs = 0;
                                                            while (n * salesItem.Length +
                                                                   (n + 1) * 5 + 180 + Lsrs <= Convert.ToDouble(maxLengthPowderAnodizing)) // променено 06.03
                                                            {
                                                                n++;
                                                                if (n > 2)
                                                                {
                                                                    Lsrs = 50;
                                                                }
                                                            }

                                                            if (n != 1)
                                                            {
                                                                attributeRow.Value = ((n - 1) * salesItem.Length + n * 5 + 180 + Lsrs).ToString();
                                                                configData.MDOAByR.Select(
                                                                        "Code='Nпр' AND OperationId='" +
                                                                        attributeRow["OperationId"].ToString() +
                                                                        "' AND MDORId='" + attributeRow["MDORId"].ToString() +
                                                                        "'")[0]["Value"] = n - 1;
                                                            }
                                                            else
                                                            {
                                                                //ConfigData.MDOAByR.Select(
                                                                //       "Code='Nпр' AND OperationId='" +
                                                                //       attributeRow["OperationId"].ToString() +
                                                                //       "' AND MDORId='" + attributeRow["MDORId"].ToString() +
                                                                //       "'")[0]["Value"] =1;
                                                                configData.MDOAByR.First(Npr => Npr.Code == "Nпр" && Npr.OperationId == attributeRow.OperationId && Npr.MDORId == attributeRow.MDORId).Value = "1";
                                                                if ((Math.Abs(Convert.ToDecimal(
                                                                         salesItem.Options["ToleranceLengthMin"])) +
                                                                     Math.Abs(Convert.ToDecimal(
                                                                         salesItem.Options["ToleranceLengthMax"]))) >= 2 &&
                                                                    (salesItem.Length + 100) <=
                                                                    Convert.ToDouble(maxLengthPowderAnodizing))  // променено 06.03
                                                                {
                                                                    attributeRow.Value = (salesItem.Length + 100).ToString();
                                                                }
                                                                else
                                                                {
                                                                    RaiseErrorIfNoOtherResources(salesItem, attributeRow, "Тази дължина без следи от елоксация не може да се пренареже");
                                                                    //attributeRow.MaterialDefinitionOperationsRow.Status = "Error";
                                                                    //attributeRow.MaterialDefinitionOperationsRow.StatusMessage = "Тази дължина без следи от елоксация не може да се пренареже";
                                                                    //SalesItem.ErrorMessage =
                                                                    //    "Тази дължина без следи от елоксация не може да се пренареже";
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            RaiseErrorIfNoOtherResources(salesItem, attributeRow, "Дължина под минималната за точен циркуляр");
                                                            //attributeRow.MaterialDefinitionOperationsRow.Status = "Error";
                                                            //attributeRow.MaterialDefinitionOperationsRow.StatusMessage = "Дължина под минималната за точен циркуляр";
                                                            //SalesItem.ErrorMessage = "Дължина под минималната за точен циркуляр";
                                                        }


                                                    }
                                                    else if (Math.Truncate(Convert.ToDecimal(
                                                                 (maxLengthPowderAnodizing - 10) /
                                                                 Convert.ToDecimal(attributeRow.Value))) == 2)
                                                    {
                                                        //if (ConfigData.MDOAByR.Select("Code = 'Nпр' AND OperationId=" + attributeRow["OperationId"])[0].IsNull("Value") || Convert.ToString(ConfigData.MDOAByR.Select("Code = 'Nпр' AND OperationId=" + attributeRow["OperationId"])[0]["Value"]) == "")
                                                        //    {
                                                        //    attributeRow.GetParentRow("FK_MaterialDefinitionOperationAttributes_ToMaterialDefinitionOperations")["Status"] = "InputWaiting";
                                                        //    attributeRow.GetParentRow("FK_MaterialDefinitionOperationAttributes_ToMaterialDefinitionOperations")["StatusMessage"] = "Трябва да избере Nпр(1 или 2) : Lsrs(0 или 50)";
                                                        //    }
                                                        //else
                                                        //    {
                                                        //    switch (Convert.ToInt32(ConfigData.MDOAByR.Select("Code = 'Nпр' AND OperationId=" + attributeRow["OperationId"])[0]["Value"]))
                                                        //        {
                                                        //        case 1:
                                                        //            attributeRow["Value"] = SalesItem.Length + 10;
                                                        //            attributeRow.GetParentRow("FK_MaterialDefinitionOperationAttributes_ToMaterialDefinitionOperations")["Status"] = "ОК";
                                                        //            attributeRow.GetParentRow("FK_MaterialDefinitionOperationAttributes_ToMaterialDefinitionOperations")["StatusMessage"] = "";
                                                        //            break;
                                                        //        case 2:
                                                        //            attributeRow["Value"] = 2 * SalesItem.Length + 60;
                                                        //            attributeRow.GetParentRow("FK_MaterialDefinitionOperationAttributes_ToMaterialDefinitionOperations")["Status"] = "ОК";
                                                        //            attributeRow.GetParentRow("FK_MaterialDefinitionOperationAttributes_ToMaterialDefinitionOperations")["StatusMessage"] = "";
                                                        //            break;
                                                        //        default:
                                                        //            attributeRow.GetParentRow("FK_MaterialDefinitionOperationAttributes_ToMaterialDefinitionOperations")["Status"] = "InputWaiting";
                                                        //            attributeRow.GetParentRow("FK_MaterialDefinitionOperationAttributes_ToMaterialDefinitionOperations")["StatusMessage"] = "Невалидна стойност на Nпр.\r\nТрябва да избере Nпр(1 или 2) : Lsrs(0 или 50)";
                                                        //            break;
                                                        //        }
                                                        //    }
                                                        attributeRow.Value = (2 * salesItem.Length + 50).ToString();
                                                        configData.MDOAByR.Select(
                                                            "Code='Nпр' AND OperationId='" +
                                                            attributeRow["OperationId"].ToString() + "' AND MDORId='" +
                                                            attributeRow["MDORId"].ToString() + "'")[0]["Value"] = 2;
                                                    }
                                                    else if (Math.Truncate(Convert.ToDecimal(
                                                                 (maxLengthPowderAnodizing - 10) /
                                                                 Convert.ToDecimal(attributeRow.Value))) == 1)
                                                    {
                                                        if (Math.Abs(Convert.ToDecimal(
                                                                salesItem.Options["ToleranceLengthMax"])) < 5)
                                                        {
                                                            attributeRow.Value =
                                                                (Convert.ToDecimal(salesItem.Options["Length"]) + Math.Abs(Convert.ToDecimal(
                                                                    salesItem.Options["ToleranceLengthMin"])) +
                                                                Math.Abs(Convert.ToDecimal(
                                                                    salesItem.Options["ToleranceLengthMax"])) - 5).ToString();
                                                        }

                                                    }
                                                }
                                                else
                                                {
                                                    //по-малка ли е от минималната дължина за рязане на пресата
                                                    if (Convert.ToDecimal(attributeRow.Value) <
                                                        Convert.ToDecimal(
                                                            definitionData.ResourcesAttributes.Select(
                                                                "ResourcesId = " + Convert.ToString(CurrResId) +
                                                                " AND Code='Lpress min'")[0]["Value"]))
                                                    {
                                                        //по-малка ли е от минималната дължина за пренарязване на точен циркуляр
                                                        if (Convert.ToDecimal(attributeRow.Value) < Convert.ToDecimal(definitionData.ResourcesAttributes.FindById(1004).Value))
                                                        {
                                                            RaiseErrorIfNoOtherResources(salesItem, attributeRow, "По-малка от минималната дължина за пренарязване");
                                                            //attributeRow.MaterialDefinitionOperationsRow.Status = "Error";
                                                            //attributeRow.MaterialDefinitionOperationsRow.StatusMessage =
                                                            //    "По-малка от минималната дължина на пренарязване";
                                                        }
                                                        else
                                                        {
                                                            int n = 0;
                                                            decimal Lmaxcs = Convert.ToDecimal(
                                                                definitionData.ResourcesAttributes.Select(
                                                                    "Code='Lmaxcs' AND ResourcesId=" +
                                                                    Convert.ToString(CurrResId))[0]["Value"]);
                                                            //В цикъла включвам втора проверка, заимствана от алгоритъма на Lpress и там търся n>1, за да не влизам в цикъла, в който да търся комбиниране с други поръчки. Трябва да се коментира с Ивелина
                                                            while (Convert.ToDecimal(attributeRow.Value) <
                                                                   Convert.ToDecimal(
                                                                       definitionData.ResourcesAttributes.Select("Id = 1005")[0]
                                                                           ["Value"])
                                                            ) // && Math.Truncate(Convert.ToDecimal(Lmaxcs - 10) / Convert.ToDecimal(attributeRow["Value"])) > 1)
                                                            {
                                                                n++;
                                                                attributeRow.Value = (n * salesItem.Length + (n + 1) * 5 + 180).ToString();
                                                            }

                                                            if (n == 0)
                                                            {
                                                                RaiseErrorIfNoOtherResources(salesItem, attributeRow, "Не може да се извърши пренарязване");
                                                                //attributeRow.MaterialDefinitionOperationsRow.Status = "Error";
                                                                //attributeRow.MaterialDefinitionOperationsRow.StatusMessage = "Не може да се извърши пренаязване";
                                                                //SalesItem.ErrorMessage = "Не може да се извърши пренаязване";
                                                            }
                                                            else
                                                            {
                                                                attributeRow.Value = ((n - 1) * salesItem.Length + n * 5 + 180).ToString();
                                                                configData.MDOAByR.Select(
                                                                        "Code='Nпр' AND OperationId='" +
                                                                        attributeRow["OperationId"].ToString() +
                                                                        "' AND MDORId='" + attributeRow["MDORId"].ToString() +
                                                                        "'")[0]["Value"] = n - 1;
                                                                //Ако имаме пренарязване да го запишем като стойност на добавена от нас операция   ConfigData.MDOAByR.Select("Code='Lzag' AND OperationId=" + attributeRow["OperationId"].ToString() + " AND MDORId=" + attributeRow["MDORId"].ToString())[0]["Value"] = Lzag;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if ((Math.Abs(
                                                                 Convert.ToDecimal(salesItem.Options["ToleranceLengthMin"])) +
                                                             Math.Abs(
                                                                 Convert.ToDecimal(salesItem.Options["ToleranceLengthMax"]))) <
                                                            5)
                                                        {
                                                            if ((Math.Abs(Convert.ToDecimal(
                                                                     salesItem.Options["ToleranceLengthMin"])) +
                                                                 Math.Abs(Convert.ToDecimal(
                                                                     salesItem.Options["ToleranceLengthMax"]))) <
                                                                Convert.ToDecimal(0.3)) //min. допуск пренарязване
                                                            {
                                                                RaiseErrorIfNoOtherResources(salesItem, attributeRow, "Допуск под минималния възможен за изпълнение");
                                                                //attributeRow.MaterialDefinitionOperationsRow.Status = "Error";
                                                                //attributeRow.MaterialDefinitionOperationsRow.StatusMessage =
                                                                //    "Допуск под минималния възможен за изпълнение";
                                                                //SalesItem.ErrorMessage =
                                                                //    "Допуск под минималния възможен за изпълнение";
                                                            }
                                                            else
                                                            {
                                                                int n = 0;
                                                                int Lmax = Convert.ToInt32(
                                                                    definitionData.ResourcesAttributes.Select(
                                                                        "Code='Lmax' AND ResourcesId=" +
                                                                        Convert.ToString(CurrResId))[0]["Value"]);
                                                                int Lmax_pren = Convert.ToInt32(
                                                                    definitionData.ResourcesAttributes.Select(
                                                                        "Code='Lmax_pren' AND ResourcesId=" +
                                                                        Convert.ToString(CurrResId))[0]["Value"]);
                                                                //В цикъла включвам втора проверка, заимствана от алгоритъма на Lpress и там търся n>1, за да не влизам в цикъла, в който да търся комбиниране с други поръчки. Трябва да се коментира с Ивелина
                                                                while (Convert.ToDouble(attributeRow.Value) < Lmax_pren)
                                                                {
                                                                    n++;
                                                                    attributeRow.Value = (n * salesItem.Length + (n + 1) * 5 + 180).ToString();
                                                                }

                                                                if (n == 0)
                                                                {
                                                                    RaiseErrorIfNoOtherResources(salesItem, attributeRow, "Не може да се извъши пренарязване");
                                                                    //attributeRow.MaterialDefinitionOperationsRow.Status = "Error";
                                                                    //attributeRow.GetParentRow(
                                                                    //        "FK_MaterialDefinitionOperationAttributes_ToMaterialDefinitionOperations")
                                                                    //    ["StatusMessage"] = "Не може да се извърши пренаязване";
                                                                    //SalesItem.ErrorMessage =
                                                                    //    "Не може да се извърши пренаязване";
                                                                }
                                                                else
                                                                {
                                                                    attributeRow.Value = ((n - 1) * salesItem.Length + n * 5 + 180).ToString();
                                                                    configData.MDOAByR.Select(
                                                                            "Code='Nпр' AND OperationId='" +
                                                                            attributeRow["OperationId"].ToString() +
                                                                            "' AND MDORId='" +
                                                                            attributeRow["MDORId"].ToString() +
                                                                            "'")[0]["Value"] = n - 1;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (Convert.ToDecimal(salesItem.Options["ToleranceLengthMax"]) > 5)
                                                            {
                                                                attributeRow.Value = salesItem.Options["Length"];
                                                            }
                                                            else
                                                            {
                                                                attributeRow.Value = (Convert.ToDecimal(salesItem.Options["Length"]) + Convert.ToDecimal(salesItem.Options["ToleranceLengthMax"]) - 5).ToString();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (Convert.ToInt32(salesItem.Options["LengthFinalProduct"]) > 0)
                                                {
                                                    attributeRow.Value = salesItem.Options["LengthFinalProduct"];
                                                }
                                            }

                                        }
                                        else
                                        {
                                            attributeRow.Value = salesItem.Options["LengthSubcontractor"];
                                        }

                                        Lprkr = attributeRow.RowState != DataRowState.Detached ? Convert.ToInt32(Convert.ToDouble(attributeRow.Value)) : 0; //TISHO - warning , to discuss with Ivelina
                                    }
                                    else
                                    {
                                        attributeRow.Value = profilesData.ProfileProduct.FindByERPItemOpNoERPVariant(salesItem.ProfileCode, 10, salesItem.VariantCode).Lprkr.ToString();
                                        Lprkr = Convert.ToInt32(profilesData.ProfileProduct.FindByERPItemOpNoERPVariant(salesItem.ProfileCode, 10, salesItem.VariantCode).Lprkr);
                                    }

                                    break;
                                //L пресоване - дължина на пресоване
                                case 2:
                                    int Lscrap = 0;
                                    if (definitionData.ResourcesAttributes
                                            .Select("Code='Lmaxcs' AND ResourcesId=" + Convert.ToString(CurrResId))
                                            .Count() > 0)
                                    {
                                        int n = 0;
                                        int Lmaxcs =
                                            Convert.ToInt32(
                                                definitionData.ResourcesAttributes.Select(
                                                    "Code='Lmaxcs' AND ResourcesId=" + Convert.ToString(CurrResId))[0][
                                                    "Value"]);
                                        int Lhv = 0;
                                        while (n * Lprkr + 500 <= Lmaxcs)
                                        {
                                            n++;
                                        }

                                        if (n > 1)
                                        {
                                            Lhv = 0;
                                            n--;
                                            int Lzk = 0;
                                            if (profilesData.ProfilesEnds
                                                    .Select("ProfileId = " + Convert.ToString(salesItem.IDinTechDB) +
                                                            " AND AlloyFamily = '" + AlloyToSearch + "'").Count() > 0)
                                            {
                                                if (profilesData.ProfilesEnds.Select(
                                                        "ProfileId = " + Convert.ToString(salesItem.IDinTechDB) +
                                                        " AND AlloyFamily = '" + AlloyToSearch + "'")[0]["LengthEnd"] !=
                                                    DBNull.Value)
                                                {
                                                    Lzk = Convert.ToInt32(
                                                        profilesData.ProfilesEnds.Select(
                                                            "ProfileId = " + Convert.ToString(salesItem.IDinTechDB) +
                                                            " AND AlloyFamily = '" + AlloyToSearch + "'")[0]["LengthEnd"]);
                                                }

                                            }

                                            int Lpr = 0;
                                            int LzagMax = Convert.ToInt32(
                                                definitionData.ResourcesAttributes.Select(
                                                    "Code='LzagMax' AND ResourcesId=" + Convert.ToString(CurrResId))[0][
                                                    "Value"]);
                                            int k = 0;
                                            int Lzag = 0;
                                            int CntZag = 0;
                                            double divider = 0;
                                            switch (CurrResId)
                                            {
                                                case 7:
                                                    divider = 8103.2096;
                                                    break;
                                                case 8:
                                                    divider = 17898.785;
                                                    break;
                                                case 37:
                                                case 9:
                                                    divider = 24593.265;
                                                    break;
                                                case 10: // Редактирано : 2019.11.07
                                                case 29:
                                                    divider = 32031.14;
                                                    break;
                                                default:
                                                    RaiseErrorIfNoOtherResources(salesItem, attributeRow, $"В кода не е добавена информация за ресурс с ИД {CurrResId}");
                                                    //attributeRow.MaterialDefinitionOperationsRow.Status = "Error";
                                                    //attributeRow.GetParentRow(
                                                    //        "FK_MaterialDefinitionOperationAttributes_ToMaterialDefinitionOperations")
                                                    //    ["StatusMessage"] +=
                                                    //"В кода не е добавена информация за ресур с ИД ";
                                                    //SalesItem.ErrorMessage =
                                                    //    "В кода не е добавена информация за ресур с ИД ";
                                                    break;
                                            }

                                            //important
                                            if (Lzk == 0)
                                            {
                                                int tmpPressLength =
                                                    Convert.ToInt32(
                                                        attributeRow.GetParentRow("FK_MDOAByR_ToMDOR")["ResourceId"]);
                                                if (profilesData.ProfilesByPress
                                                        .Select("ProfileId=" + Convert.ToString(salesItem.IDinTechDB) +
                                                                " AND AlloyFamily = '" + AlloyToSearch + "' AND PressID=" +
                                                                tmpPressLength.ToString()).Count() > 0 &&
                                                    profilesData.ProfilesByPress.Select(
                                                        "ProfileId=" + Convert.ToString(salesItem.IDinTechDB) +
                                                        " AND AlloyFamily = '" + AlloyToSearch + "' AND PressID=" +
                                                        tmpPressLength.ToString())[0]["MaxLengthPress"] !=
                                                    System.DBNull.Value)
                                                {
                                                    tmpPressLength = Convert.ToInt32(
                                                        profilesData.ProfilesByPress.Select(
                                                            "ProfileId=" + Convert.ToString(salesItem.IDinTechDB) +
                                                            " AND AlloyFamily = '" + AlloyToSearch + "'  AND PressID=" +
                                                            tmpPressLength.ToString())[0]["MaxLengthPress"]);
                                                }
                                                else
                                                {
                                                    tmpPressLength = Convert.ToInt32(
                                                        definitionData.ResourcesAttributes.Select(
                                                                "Code='PressLength' AND ResourcesId=" +
                                                                CurrResId.ToString())[0]
                                                            ["Value"]);
                                                }

                                                k = Convert.ToInt32(
                                                    Math.Truncate(Convert.ToDecimal(tmpPressLength - 1300) / Lprkr));
                                                Lscrap = 1300;
                                                Lpr = k * Lprkr + Lscrap;
                                                configData.MDOAByR.Select(
                                                    "Code='Lscrap' AND OperationId='" +
                                                    attributeRow["OperationId"].ToString() + "' AND MDORId='" +
                                                    attributeRow["MDORId"].ToString() + "'")[0]["Value"] = 1300;

                                                if (divider > 0 && profilesData.ProfilesByPress
                                                        .Select("ProfileId=" + Convert.ToString(salesItem.IDinTechDB) +
                                                                " AND AlloyFamily = '" + AlloyToSearch + "'  AND PressID=" +
                                                                CurrResId.ToString()).Count() > 0)
                                                {
                                                    Lzag = Convert.ToInt32(
                                                               1.015 * Lpr *
                                                               profilesData.Profiles.FindByProfileId(salesItem.IDinTechDB)
                                                                   .Section * Convert.ToInt32(
                                                                   profilesData.ProfilesByPress.Select(
                                                                       "ProfileId=" +
                                                                       Convert.ToString(salesItem.IDinTechDB) +
                                                                       " AND AlloyFamily = '" + AlloyToSearch +
                                                                       "' AND PressID=" + CurrResId.ToString())[0][
                                                                       "Channels"]) / divider) +
                                                           Convert.ToInt32(
                                                               definitionData.ResourcesAttributes.Select(
                                                                   "Code='presostatyk' AND ResourcesId=" +
                                                                   Convert.ToString(CurrResId))[0]["Value"]);
                                                    while (Lzag > LzagMax)
                                                    {
                                                        k--;
                                                        Lpr = k * Lprkr + Lscrap;
                                                        configData.MDOAByR.Select(
                                                            "Code='Lscrap' AND OperationId='" +
                                                            attributeRow["OperationId"].ToString() + "' AND MDORId='" +
                                                            attributeRow["MDORId"].ToString() + "'")[0]["Value"] = 1300;
                                                        Lzag = Convert.ToInt32(
                                                                   1.015 * Lpr *
                                                                   profilesData.Profiles
                                                                       .FindByProfileId(salesItem.IDinTechDB).Section *
                                                                   Convert.ToInt32(
                                                                       profilesData.ProfilesByPress.Select(
                                                                           "ProfileId=" +
                                                                           Convert.ToString(salesItem.IDinTechDB) +
                                                                           " AND AlloyFamily = '" + AlloyToSearch +
                                                                           "'  AND PressID=" + CurrResId.ToString())[0][
                                                                           "Channels"]) / divider) +
                                                               Convert.ToInt32(
                                                                   definitionData.ResourcesAttributes.Select(
                                                                       "Code='presostatyk' AND ResourcesId=" +
                                                                       Convert.ToString(CurrResId))[0]["Value"]);
                                                    }

                                                    if (k > 0)
                                                    {
                                                        if (k == n)

                                                        {
                                                            Lscrap = 800;
                                                            Lpr = k * Lprkr + Lscrap;
                                                            configData.MDOAByR.Select(
                                                                "Code='Lscrap' AND OperationId='" +
                                                                attributeRow["OperationId"].ToString() + "' AND MDORId='" +
                                                                attributeRow["MDORId"].ToString() + "'")[0]["Value"] = 800;
                                                        }

                                                        attributeRow.Value = Lpr.ToString();
                                                        configData.MDOAByR.Select("Code='Lzag' AND OperationId='" + attributeRow["OperationId"].ToString() + "' AND MDORId='" +
                                                                attributeRow["MDORId"].ToString() + "'")[0]["Value"] =
                                                            Convert.ToInt32(Lzag);
                                                        //тук зависим от количеството в поръчката за продажба и не би трябвало да е в дефиницията на продукта - да се измести после при генерацията на ПП
                                                        CntZag = Convert.ToInt32(Math.Ceiling(
                                                            (salesItem.Quantity * 1.05) /
                                                            (2.7 * k * Lprkr *
                                                             profilesData.Profiles.FindByProfileId(salesItem.IDinTechDB)
                                                                 .Section * Convert.ToInt32(
                                                                 profilesData.ProfilesByPress.Select(
                                                                         "ProfileId=" +
                                                                         Convert.ToString(salesItem.IDinTechDB) +
                                                                         " AND AlloyFamily = '" + AlloyToSearch +
                                                                         "' AND PressID=" + CurrResId.ToString())[0]
                                                                     ["Channels"])) * 1000 * 1000));
                                                        configData.MDOAByR.Select(
                                                            "Code='CntZag' AND OperationId='" +
                                                            attributeRow["OperationId"].ToString() + "' AND MDORId='" +
                                                            attributeRow["MDORId"].ToString() + "'")[0]["Value"] = CntZag;
                                                        if (Spr > 0)
                                                        {
                                                            attributeRow.MaterialDefinitionOperationResourcesRow.Duration = Lpr * CntZag * 60 / (1000 * Spr) + CntZag * 40; //(Lpr * CntZag / (1000 * Spr) + CntZag) * 60;

                                                            //допълнителна проверка - щом сме стигнали до тук , то значи операцията може да се изпълни. Ако за друг ресурс е имало грешка, то ще я изчистим. Да се помисли как да се действа за в бъдеще
                                                            if (operationRow.Status == "Error" &&
                                                                salesItem.ErrorMessage != "")
                                                            {
                                                                operationRow.Status = "OK";
                                                                salesItem.ErrorMessage = "";
                                                            }

                                                        }
                                                        else
                                                        {
                                                            RaiseErrorIfNoOtherResources(salesItem, attributeRow, "Speed - липса на данни в технологичната база данни");
                                                            break;
                                                            //attributeRow.MaterialDefinitionOperationsRow.Status = "Error";
                                                            //attributeRow.GetParentRow(
                                                            //            "FK_MaterialDefinitionOperationAttributes_ToMaterialDefinitionOperations")
                                                            //        ["StatusMessage"] +=
                                                            //    "Speed - липса на данни в технологичната база данни";
                                                            //SalesItem.ErrorMessage =
                                                            //    "Speed - липса на данни в технологичната база данни";
                                                            //var AvgSpeed = from pd in ProfilesData.ProfilesByPress join p in ProfilesData.Profiles on pd.ProfileId equals p.ProfileId where p.Section >;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        RaiseErrorIfNoOtherResources(salesItem, attributeRow, $"К=0 коефициент при изчисление на Lpress  за ресурс {definitionData.Resources.FindByResourcesId(CurrResId).Name}");
                                                        break;
                                                        //attributeRow.MaterialDefinitionOperationsRow.Status = "Error";
                                                        //attributeRow.MaterialDefinitionOperationsRow.StatusMessage += $"К=0 коефициент при изчисление на Lpress за ресурс {DefinitionData.Resources.FindByResourcesId(CurrResId).Name}";
                                                        //SalesItem.ErrorMessage = $"К=0 коефициент при изчисление на Lpress  за ресурс {DefinitionData.Resources.FindByResourcesId(CurrResId).Name}";
                                                    }
                                                }
                                                else
                                                {
                                                    RaiseErrorIfNoOtherResources(salesItem, attributeRow, $"Липсващ профил в технологичната база данни");
                                                    break;
                                                    //attributeRow.MaterialDefinitionOperationsRow.Status = "Error";
                                                    //attributeRow.GetParentRow(
                                                    //        "FK_MaterialDefinitionOperationAttributes_ToMaterialDefinitionOperations")
                                                    //    ["StatusMessage"] += "Липсващ профил в технологичната база данни";
                                                    //SalesItem.ErrorMessage = "Липсващ профил в технологичната база данни";
                                                }
                                            }
                                            else
                                            {
                                                //if (n * Lprkr + Lzk + 400 > Lmaxcs)
                                                //    {
                                                //    Lpr = n * Lprkr + Lzk + 400;
                                                //    ConfigData.MDOAByR.Select("Code='Lscrap' AND OperationId='" + attributeRow["OperationId"].ToString() + "' AND MDORId='" + attributeRow["MDORId"].ToString() + "'")[0]["Value"] = Lzk + 400;
                                                //    attributeRow["Value"] = Lpr;
                                                //    Lzag = Convert.ToInt32(1.015 * Lpr * ProfilesData.Profiles.FindByProfileId(SalesItem.IDinTechDB).Section * Convert.ToInt32(ProfilesData.ProfilesByPress.Select("ProfileId=" + Convert.ToString(SalesItem.IDinTechDB) + " AND PressID=" + CurrResId.ToString())[0]["Channels"]) / divider) + Convert.ToInt32(DefinitionData.ResourcesAttributes.Select("Code='presostatyk' AND ResourcesId=" + Convert.ToString(CurrResId))[0]["Value"]);
                                                //    ConfigData.MDOAByR.Select("Code='Lzag' AND OperationId='" + attributeRow["OperationId"].ToString() + "' AND MDORId='" + attributeRow["MDORId"].ToString() + "'")[0]["Value"] = Convert.ToInt32(Lzag );
                                                //    CntZag = Convert.ToInt32(Math.Ceiling((SalesItem.Quantity * 1.05) / (2.7 * Lprkr * ProfilesData.Profiles.FindByProfileId(SalesItem.IDinTechDB).Section * Convert.ToInt32(ProfilesData.ProfilesByPress.Select("ProfileId=" + Convert.ToString(SalesItem.IDinTechDB) + " AND PressID=" + CurrResId.ToString())[0]["Channels"])) * 1000 * 1000));
                                                //    ConfigData.MDOAByR.Select("Code='CntZag' AND OperationId='" + attributeRow["OperationId"].ToString() + "' AND MDORId='" + attributeRow["MDORId"].ToString() + "'")[0]["Value"] = CntZag;
                                                //    if (Spr > 0)
                                                //        {
                                                //        attributeRow.GetParentRow("FK_MDOAByR_ToMDOR")["Duration"] = (Lpr * CntZag / (1000 * Spr) + 5 + CntZag) * 60;
                                                //        }
                                                //    else
                                                //        {
                                                //        attributeRow.GetParentRow("FK_MaterialDefinitionOperationAttributes_ToMaterialDefinitionOperations")["Status"] = "Error";
                                                //        attributeRow.GetParentRow("FK_MaterialDefinitionOperationAttributes_ToMaterialDefinitionOperations")["StatusMessage"] += "Speed - липса на данни в технологичната база данни";
                                                //        //var AvgSpeed = from pd in ProfilesData.ProfilesByPress join p in ProfilesData.Profiles on pd.ProfileId equals p.ProfileId where p.Section >;
                                                //        }
                                                //    operationRow["StatusMessage"] = "Рязане на засечка";
                                                //    }
                                                //else
                                                //    {
                                                int tmpPressLength =
                                                    Convert.ToInt32(
                                                        attributeRow.GetParentRow("FK_MDOAByR_ToMDOR")["ResourceId"]);
                                                if (profilesData.ProfilesByPress
                                                        .Select("ProfileId=" + Convert.ToString(salesItem.IDinTechDB) +
                                                                " AND AlloyFamily = '" + AlloyToSearch + "' AND PressID=" +
                                                                tmpPressLength.ToString()).Count() > 0 &&
                                                    profilesData.ProfilesByPress.Select("ProfileId=" + Convert.ToString(salesItem.IDinTechDB) +
                                                        " AND AlloyFamily = '" + AlloyToSearch + "' AND PressID=" +
                                                        tmpPressLength.ToString())[0]["MaxLengthPress"] !=
                                                    System.DBNull.Value)
                                                {
                                                    tmpPressLength = Convert.ToInt32(
                                                        profilesData.ProfilesByPress.Select(
                                                            "ProfileId=" + Convert.ToString(salesItem.IDinTechDB) +
                                                            " AND AlloyFamily = '" + AlloyToSearch + "'  AND PressID=" +
                                                            tmpPressLength.ToString())[0]["MaxLengthPress"]);
                                                }
                                                else
                                                {
                                                    tmpPressLength = Convert.ToInt32(
                                                        definitionData.ResourcesAttributes.Select(
                                                                "Code='PressLength' AND ResourcesId=" +
                                                                CurrResId.ToString())[0]
                                                            ["Value"]);
                                                }

                                                k = Convert.ToInt32(Math.Truncate(Convert.ToDecimal(
                                                    (Convert.ToInt32(tmpPressLength) - Lzk - 1000) /
                                                    Convert.ToDecimal(Lprkr))));
                                                Lscrap = Lzk + 1000;
                                                Lpr = k * Lprkr + Lscrap;

                                                configData.MDOAByR.Select(
                                                        "Code='Lscrap' AND OperationId='" +
                                                        attributeRow["OperationId"].ToString() + "' AND MDORId='" +
                                                        attributeRow["MDORId"].ToString() + "'")[0]["Value"] = Lzk + 1000;
                                                if (divider > 0 && profilesData.ProfilesByPress
                                                        .Select("ProfileId=" + Convert.ToString(salesItem.IDinTechDB) +
                                                                " AND AlloyFamily = '" + AlloyToSearch + "' AND PressID=" +
                                                                CurrResId.ToString()).Count() > 0)
                                                {
                                                    Lzag = Convert.ToInt32(
                                                               1.015 * Lpr *
                                                               profilesData.Profiles.FindByProfileId(salesItem.IDinTechDB)
                                                                   .Section * Convert.ToInt32(
                                                                   profilesData.ProfilesByPress.Select(
                                                                       "ProfileId=" +
                                                                       Convert.ToString(salesItem.IDinTechDB) +
                                                                       " AND AlloyFamily = '" + AlloyToSearch +
                                                                       "' AND PressID=" + CurrResId.ToString())[0][
                                                                       "Channels"]) / divider) +
                                                           Convert.ToInt32(
                                                               definitionData.ResourcesAttributes.Select(
                                                                   "Code='presostatyk' AND ResourcesId=" +
                                                                   Convert.ToString(CurrResId))[0]["Value"]);
                                                }

                                                while (Lzag > LzagMax)
                                                {
                                                    k--;
                                                    if (k == n)
                                                    {
                                                        Lscrap = Lzk + 400;
                                                    }
                                                    else
                                                    {
                                                        Lscrap = Lzk + 1000;
                                                    }

                                                    Lpr = k * Lprkr + Lscrap;
                                                    configData.MDOAByR.Select(
                                                            "Code='Lscrap' AND OperationId='" +
                                                            attributeRow["OperationId"].ToString() + "' AND MDORId='" +
                                                            attributeRow["MDORId"].ToString() + "'")[0]["Value"] =
                                                        Lzk + 1000;
                                                    Lzag = Convert.ToInt32(
                                                               1.015 * Lpr *
                                                               profilesData.Profiles.FindByProfileId(salesItem.IDinTechDB)
                                                                   .Section * Convert.ToInt32(
                                                                   profilesData.ProfilesByPress.Select(
                                                                       "ProfileId=" +
                                                                       Convert.ToString(salesItem.IDinTechDB) +
                                                                       " AND AlloyFamily = '" + AlloyToSearch +
                                                                       "' AND PressID=" + CurrResId.ToString())[0][
                                                                       "Channels"]) / divider) +
                                                           Convert.ToInt32(
                                                               definitionData.ResourcesAttributes.Select(
                                                                   "Code='presostatyk' AND ResourcesId=" +
                                                                   Convert.ToString(CurrResId))[0]["Value"]);
                                                }

                                                if (k > 0)
                                                {
                                                    if (k == n)
                                                    {
                                                        Lscrap = Lzk + 400;
                                                        Lpr = k * Lprkr + Lscrap;
                                                        configData.MDOAByR.Select(
                                                                "Code='Lscrap' AND OperationId='" +
                                                                attributeRow["OperationId"].ToString() + "' AND MDORId='" +
                                                                attributeRow["MDORId"].ToString() + "'")[0]["Value"] =
                                                            Lzk + 400;
                                                    }
                                                    else
                                                    {
                                                        Lscrap = Lzk + 1000;
                                                        Lpr = k * Lprkr + Lscrap;
                                                        configData.MDOAByR.Select(
                                                                "Code='Lscrap' AND OperationId='" +
                                                                attributeRow["OperationId"].ToString() + "' AND MDORId='" +
                                                                attributeRow["MDORId"].ToString() + "'")[0]["Value"] =
                                                            Lzk + 1000;
                                                    }

                                                    attributeRow.Value = Lpr.ToString();
                                                    configData.MDOAByR.Select(
                                                            "Code='Lzag' AND OperationId='" +
                                                            attributeRow["OperationId"].ToString() + "' AND MDORId='" +
                                                            attributeRow["MDORId"].ToString() + "'")[0]["Value"] =
                                                        Convert.ToInt32(Lzag);
                                                    //тук зависим от количеството в поръчката за продажба и не би трябвало да е в дефиницията на продукта - да се измести после при генерацията на ПП
                                                    if (profilesData.ProfilesByPress
                                                            .Select("ProfileId=" + Convert.ToString(salesItem.IDinTechDB) +
                                                                    " AND AlloyFamily = '" + AlloyToSearch +
                                                                    "' AND PressID=" + CurrResId.ToString()).Count() > 0)
                                                    {
                                                        CntZag = Convert.ToInt32(Math.Ceiling(
                                                            (salesItem.Quantity * 1.05) /
                                                            (2.7 * k * Lprkr *
                                                             profilesData.Profiles.FindByProfileId(salesItem.IDinTechDB)
                                                                 .Section * Convert.ToInt32(
                                                                 profilesData.ProfilesByPress.Select(
                                                                         "ProfileId=" +
                                                                         Convert.ToString(salesItem.IDinTechDB) +
                                                                         " AND AlloyFamily = '" + AlloyToSearch +
                                                                         "' AND PressID=" + CurrResId.ToString())[0]
                                                                     ["Channels"])) * 1000 * 1000));
                                                    }
                                                    else
                                                    {
                                                        RaiseErrorIfNoOtherResources(salesItem, attributeRow, "CntZag - липса на данни за брой канали за този профил, сплав и преса");
                                                        break;
                                                        //attributeRow.MaterialDefinitionOperationsRow.Status = "Error";
                                                        //attributeRow.GetParentRow(
                                                        //            "FK_MaterialDefinitionOperationAttributes_ToMaterialDefinitionOperations")
                                                        //        ["StatusMessage"] +=
                                                        //    "CntZag - липса на данни за брой канали за този профил, сплав и преса";
                                                        //SalesItem.ErrorMessage =
                                                        //    "CntZag - липса на данни за брой канали за този профил, сплав и преса";
                                                    }

                                                    if (configData.MDOAByR.Any(a => a.Code == "CntZag" && a.OperationId == attributeRow.OperationId && a.MDORId == attributeRow.MDORId))
                                                    { configData.MDOAByR.First(a => a.Code == "CntZag" && a.OperationId == attributeRow.OperationId && a.MDORId == attributeRow.MDORId).Value = CntZag.ToString(); }
                                                    else
                                                    {
                                                        RaiseErrorIfNoOtherResources(salesItem, attributeRow, "CntZag - липса на данни за брой канали за този профил, сплав и преса");
                                                        break;
                                                    }

                                                    if (Spr > 0)
                                                    {
                                                        attributeRow.MaterialDefinitionOperationResourcesRow.Duration = Lpr * CntZag * 60 / (1000 * Spr) + CntZag * 40; // (Lpr * CntZag / (1000 * Spr) + CntZag) * 60;
                                                    }
                                                    else
                                                    {
                                                        RaiseErrorIfNoOtherResources(salesItem, attributeRow, "Speed - липса на данни в технологичната база данни");
                                                        //attributeRow.MaterialDefinitionOperationsRow.Status = "Error";
                                                        //attributeRow.GetParentRow(
                                                        //            "FK_MaterialDefinitionOperationAttributes_ToMaterialDefinitionOperations")
                                                        //        ["StatusMessage"] +=
                                                        //    "Speed - липса на данни в технологичната база данни";
                                                        //SalesItem.ErrorMessage =
                                                        //    "Speed - липса на данни в технологичната база данни";
                                                        //var AvgSpeed = from pd in ProfilesData.ProfilesByPress join p in ProfilesData.Profiles on pd.ProfileId equals p.ProfileId where p.Section >;
                                                    }
                                                }
                                                else
                                                {
                                                    RaiseErrorIfNoOtherResources(salesItem, attributeRow, "К=0 коефициент при изчисление на Lpress");
                                                    break;
                                                    //attributeRow.MaterialDefinitionOperationsRow.Status = "Error";
                                                    //attributeRow.MaterialDefinitionOperationsRow.StatusMessage += "К=0 коефициент при изчисление на Lpress";
                                                    //SalesItem.ErrorMessage = "К=0 коефициент при изчисление на Lpress";
                                                }
                                            }

                                            //}
                                        }
                                        else
                                        {
                                            //АКО ТЪРСИМ ПОРЪЧКИ
                                            //attributeRow.GetParentRow("FK_MaterialDefinitionOperationAttributes_ToMaterialDefinitionOperations")["Status"] = "Error";
                                            //attributeRow.GetParentRow("FK_MaterialDefinitionOperationAttributes_ToMaterialDefinitionOperations")["StatusMessage"] = "Тази дължина не може да се пренареже";
                                        }
                                    }
                                    else
                                    {
                                        RaiseErrorIfNoOtherResources(salesItem, attributeRow, "Липсва параметър Lmaxsc за избрания ресурс");
                                        //attributeRow.MaterialDefinitionOperationsRow.Status = "Error";
                                        //attributeRow.MaterialDefinitionOperationsRow.StatusMessage = "Липсва параметър Lmaxsc за избрания ресурс.";
                                        //SalesItem.ErrorMessage = "Липсва параметър Lmaxsc за избрания ресурс.";
                                    }

                                    break;
                                ////L заг - дължина на заготовка
                                //case 3:
                                //    TimeToAdd = 5400;
                                //    break;
                                //Брой пренарязвания
                                case 1010:
                                    //Не е изделие, влизаме в калкулации
                                    if (!profilesData.ProfileProduct.Any(a => a.ERPItem == salesItem.ProfileCode && a.ERPVariant == salesItem.VariantCode))
                                    {
                                        if (attributeRow.IsValueNull() || attributeRow.Value == string.Empty) //ConfigData.MDOAByR.Select("Code='Nпр' AND OperationId='" + attributeRow["OperationId"].ToString() + "' AND MDORId='" + attributeRow["MDORId"].ToString() + "'")[0] ["Value"] == System.DBNull.Value ||  ConfigData.MDOAByR.Select(  "Code='Nпр' AND OperationId='" + attributeRow["OperationId"].ToString() + "' AND MDORId='" + attributeRow["MDORId"].ToString() + "'")[0] ["Value"] .ToString() == ""
                                        {
                                            attributeRow.Value = "0";
                                        }

                                        if (Convert.ToInt32(salesItem.Options["LengthSubcontractor"]) != 0)
                                        {
                                            attributeRow.Value = (Convert.ToInt32(salesItem.Options["LengthSubcontractor"]) / (5 + Convert.ToInt32(salesItem.Options["Length"]))).ToString();
                                        }

                                        if (Convert.ToInt32(attributeRow.Value) > 0 && !salesItem.MaterialDefinition.GetMaterialDefinitionOperationsRows().Any(cut => cut.OperationId == 4) && (!salesItem.MaterialDefinition.GetMaterialDefinitionOperationsRows().Any(cut => cut.OperationId == 8) || !salesItem.MaterialDefinition.GetMaterialDefinitionOperationsRows().First(cut => cut.OperationId == 8).GetMaterialDefinitionOperationResourcesRows().Any(elumatec => elumatec.ResourceId == 33)))
                                        {
                                            var cnt = salesItem.MaterialDefinition.GetMaterialDefinitionOperationsRows().Max(maxOpNo => maxOpNo.OpNumber) - 5;
                                            var newCutOperation = configData.MaterialDefinitionOperations.AddMaterialDefinitionOperationsRow(Guid.NewGuid(),
                                               salesItem.MaterialDefinition, 4, definitionData.OperationDefinition.FindById(4).Description, 1,
                                               "КГ", "OK", "", definitionData.OperationDefinition.FindById(4).DefaultResourceGroup, 0, Convert.ToInt32(Convert.ToDecimal(salesItem.Quantity) / 160 * 3600), 0,
                                               cnt);
                                            int resGroupId = definitionData.ResourceGroups.Any(a => a.Name == newCutOperation.WorkCenter) ? definitionData.ResourceGroups.First(a => a.Name == newCutOperation.WorkCenter).ResourceGroupsId : 0;
                                            foreach (var item in definitionData.ResourceGroupsResources.Where(w => w.ResourceGroupsId == resGroupId))
                                            {
                                                configData.MaterialDefinitionOperationResources.AddMaterialDefinitionOperationResourcesRow(Guid.NewGuid(), newCutOperation, item.Resources, item.GetParentRow("Resources_ResourceGroupsResources")["ID"].ToString(), definitionData.Resources.FindByResourcesId(item.Resources).Name, newCutOperation.ExecutionTime, "", 1, item.GetParentRow("Resources_ResourceGroupsResources")["ID"].ToString());
                                            }

                                        }
                                        operationRow.Quantity = Convert.ToDecimal(salesItem.Quantity * (1 + (1 - (Convert.ToInt32(attributeRow.Value) == 0 ? 1 : Convert.ToInt32(attributeRow.Value)) * salesItem.Length / Lprkr)));
                                    }
                                    else
                                    {
                                        int numberOfPiecesApprox = Convert.ToInt32(Math.Ceiling(salesItem.Quantity / (profilesData.ProfileProduct.FindByERPItemOpNoERPVariant(salesItem.ProfileCode, 10, salesItem.VariantCode).WeightPerPiece / 1000)));
                                        var kgLprkr = Lprkr * profilesData.Profiles.First(profile => profile.ProfileNo == salesItem.ProfileName).Section * 2.7 / 1000 / 1000;
                                        var Npr = profilesData.ProfileProduct.FindByERPItemOpNoERPVariant(salesItem.ProfileCode, 10, salesItem.VariantCode).Npr;
                                        attributeRow.Value = Npr.ToString();
                                        operationRow.Quantity = Convert.ToInt32((numberOfPiecesApprox / Npr) * kgLprkr); // редактирано : 2019.07.02
                                    }



                                    break;
                                //Скорост на изтичане
                                case 1013:
                                    if (attributeRow.IsValueNull())
                                    {
                                        attributeRow.Value = "0";
                                        RaiseErrorIfNoOtherResources(salesItem, attributeRow, "Speed - Не може да бъде открита информация за скорост на изтичане");
                                        //attributeRow.MaterialDefinitionOperationsRow.Status = "Error";
                                        //attributeRow.MaterialDefinitionOperationsRow.StatusMessage += "Speed - Не може да бъде открита информация за скорост на изтичане";
                                        //SalesItem.ErrorMessage = "Speed - Не може да бъде открита информация за скорост на изтичане";
                                    }

                                    break;
                                //Брой канали
                                case 2013:

                                    int tmpResCh =
                                        Convert.ToInt32(attributeRow.GetParentRow("FK_MDOAByR_ToMDOR")["ResourceId"]);
                                    if (profilesData.ProfilesByPress
                                            .Select("ProfileId=" + Convert.ToString(salesItem.IDinTechDB) +
                                                    " AND AlloyFamily = '" + AlloyToSearch + "' AND PressID=" +
                                                    tmpResCh.ToString()).Count() > 0 &&
                                        profilesData.ProfilesByPress.Select(
                                            "ProfileId=" + Convert.ToString(salesItem.IDinTechDB) + " AND AlloyFamily = '" +
                                            AlloyToSearch + "' AND PressID=" + tmpResCh.ToString())[0]["Channels"] !=
                                        System.DBNull.Value)
                                    {
                                        attributeRow.Value = (Convert.ToInt32(
                                            profilesData.ProfilesByPress.Select(
                                                "ProfileId=" + Convert.ToString(salesItem.IDinTechDB) +
                                                " AND AlloyFamily = '" + AlloyToSearch + "'  AND PressID=" +
                                                tmpResCh.ToString())[0]["Channels"])).ToString();
                                    }
                                    else
                                    {
                                        RaiseErrorIfNoOtherResources(salesItem, attributeRow, "Ch - Не може да бъде открита информация за брой канали на преса");
                                        //attributeRow.MaterialDefinitionOperationsRow.Status = "Error";
                                        //attributeRow.MaterialDefinitionOperationsRow.StatusMessage += "Ch - Не може да бъде открита информация за брой канали на преса";
                                        //SalesItem.ErrorMessage = "Ch - Не може да бъде открита информация за брой канали на преса";
                                    }

                                    break;
                                //Дължина на заден край
                                case 2014:
                                    if (profilesData.ProfilesEnds
                                            .Select("ProfileId = " + Convert.ToString(salesItem.IDinTechDB) +
                                                    " AND AlloyFamily = '" + AlloyToSearch + "'").Count() > 0)
                                    {
                                        if (profilesData.ProfilesEnds.Select(
                                                "ProfileId = " + Convert.ToString(salesItem.IDinTechDB) +
                                                " AND AlloyFamily = '" + AlloyToSearch + "'")[0]["LengthEnd"] !=
                                            DBNull.Value)
                                        {
                                            attributeRow.Value = (Convert.ToInt32(
                                                profilesData.ProfilesEnds.Select(
                                                    "ProfileId = " + Convert.ToString(salesItem.IDinTechDB) +
                                                    " AND AlloyFamily = '" + AlloyToSearch + "'")[0]["LengthEnd"])).ToString();
                                        }

                                    }
                                    else
                                    {
                                        attributeRow.Value = "0";
                                    }

                                    break;
                                //Допуск за пресоване 
                                case 2015:
                                    if (Math.Abs(Convert.ToDecimal(salesItem.Options["ToleranceLengthMax"])) > 5)
                                    {
                                        attributeRow.Value = salesItem.Options["ToleranceLengthMax"];
                                    }
                                    else
                                    {
                                        if (Math.Abs(Convert.ToDecimal(salesItem.Options["ToleranceLengthMin"])) +
                                            Math.Abs(Convert.ToDecimal(salesItem.Options["ToleranceLengthMax"])) < 5)
                                        {
                                            attributeRow.Value = "10";
                                        }
                                        else
                                        {
                                            attributeRow.Value = "5";
                                        }
                                    }

                                    break;

                                case 2071:

                                   var sidewidth =  profilesData.Profiles.FindByProfileId(salesItem.IDinTechDB).SideWidth;

                                    var Lsb = definitionData.ResourcesAttributes.Select(
                                                    "Code='Lsb' AND ResourcesId=" + Convert.ToString(CurrResId))[0]["Value"];

                                    var Tfree = definitionData.ResourcesAttributes.Select(
                                                    "Code='Tfree' AND ResourcesId=" + Convert.ToString(CurrResId))[0]["Value"];

                                    var Vsb = definitionData.ResourcesAttributes.Select(
                                                    "Code='Vsb' AND ResourcesId=" + Convert.ToString(CurrResId))[0]["Value"];

                                    if (sidewidth == 0 || sidewidth == null)
                                    {
                                        RaiseErrorIfNoOtherResources(salesItem, attributeRow, "m - Не е посочен параметър “m” ");
                                    }
                                    else
                                    {
                                        var nn =Convert.ToInt32(((Convert.ToDecimal(Lsb) - Convert.ToDecimal(sidewidth)) / Convert.ToDecimal(sidewidth) / 2));

                                        attributeRow.Value = nn.ToString();

                                        var G = Convert.ToDecimal((Convert.ToInt32(profilesData.Profiles.FindByProfileId(salesItem.IDinTechDB).Section) * 2.7) / 1000);

                                        var Tsb = Convert.ToInt32(salesItem.Quantity * (Lprkr + Convert.ToDecimal(Tfree)) / nn / Convert.ToDecimal(Vsb) / Lprkr / G) * 60;

                                        attributeRow.MaterialDefinitionOperationResourcesRow.Duration = Tsb;
                                    }
                                    
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void RaiseErrorIfNoOtherResources(ProfileProduct item, dsConfigurator.MDOAByRRow attributeRow, string message)
        {
            if (attributeRow.MaterialDefinitionOperationsRow.GetMaterialDefinitionOperationResourcesRows().Count() == 1)
            {
                attributeRow.MaterialDefinitionOperationsRow.Status = "Error";
                attributeRow.MaterialDefinitionOperationsRow.StatusMessage = message;
                if (item.ErrorMessage == string.Empty)
                {
                    item.ErrorMessage = message;
                }
                else
                {
                    item.ErrorMessage = item.ErrorMessage + Environment.NewLine + message;
                }
            }
            else
            {
                attributeRow.MaterialDefinitionOperationResourcesRow.Delete();
            }
        }

        public void CreateBomScript(List<ProfileProduct> salesItems, dsConfigurator ConfigData, dsResources DefinitionData, dsProductData ProfilesData)
        {
            foreach (SmartApps.MES.Data.ProfileProduct salesItem in salesItems)
            {
                foreach (var dataRow in salesItem.MaterialDefinition.GetChildRows("MatDef_MatDefOps"))
                {
                    var operationRow = (dsConfigurator.MaterialDefinitionOperationsRow)dataRow;
                    switch (operationRow.OperationId)
                    {
                        //Пресоване
                        case 1:
                            foreach (DataRow dataRow1 in operationRow.GetChildRows("MaterialDefinitionOperations_MaterialDefinitionOperationResources"))
                            {
                                var possibleResRow = (dsConfigurator.MaterialDefinitionOperationResourcesRow)dataRow1;
                                Guid materialGuidToAdd;
                                //брой заготовки по дължина в метри
                                double quantity = 0.001 * Convert.ToInt32(possibleResRow.GetChildRows("FK_MDOAByR_ToMDOR").First(w => Convert.ToInt32(w["AttributeId"]) == 4)["Value"]) * Convert.ToInt32(possibleResRow.GetChildRows("FK_MDOAByR_ToMDOR").First(w => Convert.ToInt32(w["AttributeId"]) == 3)["Value"]);
                                switch (possibleResRow.ResourceCode)
                                {
                                    case "ES-EE-600":
                                        materialGuidToAdd = Guid.Parse("328F4128-470E-458D-8B3F-8AE9AF6BC7E0");
                                        quantity = quantity * 22.063;
                                        break;
                                    case "ES-EE-1300":
                                        materialGuidToAdd = Guid.Parse("8CE49BBE-EC77-4BF9-835D-92AAC42622DD");
                                        quantity = quantity * 48.351;
                                        break;
                                    case "ES-EE-1800":
                                        materialGuidToAdd = Guid.Parse("AC6B3290-DCD1-4F96-A20A-3D6A6E5515CC");
                                        quantity = quantity * 66.436;
                                        break;
                                    case "ES-EE-2000":
                                    case "ES-EE-2500":
                                        quantity = quantity * 86.528;
                                        materialGuidToAdd = Guid.Parse("14B99B84-C46D-46BC-AECF-0F950AF051EB");
                                        break;
                                    default:
                                        quantity = quantity * 86.528;
                                        materialGuidToAdd = Guid.Parse("14B99B84-C46D-46BC-AECF-0F950AF051EB");
                                        break;
                                }
                                ConfigData.MaterialDefinitionComponent.AddMaterialDefinitionComponentRow(
                                                                           Guid.NewGuid(), salesItem.MaterialDefinition, operationRow, possibleResRow, (ConfigData.MaterialDefinitionComponent.Count + 1) * 10, materialGuidToAdd, Convert.ToInt32(quantity), 7, 4, 30, salesItem.Options["Alloy"], null);
                                ConfigData.MaterialDefinitionComponent.AddMaterialDefinitionComponentRow(
                                           Guid.NewGuid(), salesItem.MaterialDefinition, operationRow, possibleResRow, (ConfigData.MaterialDefinitionComponent.Count + 1) * 10, Guid.Parse("910B96AF-70A4-4C96-B9E6-51C1A680EFC5"), salesItem.Quantity - Convert.ToInt32(quantity), 7, 4, 30, salesItem.Options["Alloy"], null);
                            }
                            break;

                    }
                }
            }
        }
    }
}
