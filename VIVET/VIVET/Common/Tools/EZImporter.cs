using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Ehr.Auth;
using Ehr.Bussiness;
using Ehr.Common.Constraint;
using Ehr.Common.UI;
using Ehr.Data;
using Ehr.Models;
using LinqToExcel;

namespace Ehr.Common.Tools
{
    public class EZImporter
    {
        /// <summary>
        /// Import questionnaire from the file Excel template
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string ImportQuestionnaire(string filePath,User user, UnitWork unitWork)
        {
            StringBuilder strValidations = new StringBuilder(string.Empty);
            try
            {
                List<Questionnaire> questionnaires = new List<Questionnaire>();
                string[] lsunitvolumn = { "lít", "kg", "mg", "g", "UI", "IU", "ml" };
                if (filePath.Length > 0)
                {
                    DataSet ds = new DataSet();

                    string ConnectionString = "";
                    if (filePath.EndsWith(".xls"))
                    {
                        ConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", filePath);
                    }
                    else if (filePath.EndsWith(".xlsx"))
                    {
                        ConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", filePath);
                    }
                    using (OleDbConnection conn = new System.Data.OleDb.OleDbConnection(ConnectionString))
                    {
                        conn.Open();
                        using (DataTable dtExcelSchema = conn.GetSchema("Tables"))
                        {
                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            string query = "SELECT * FROM [" + sheetName + "$A1:IB12000]";
                            OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                            //DataSet ds = new DataSet();
                            adapter.Fill(ds, "Items");
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)//ds.Tables[0].Rows.Count
                                    {
                                        Questionnaire questionnaire = new Questionnaire();
                                        DataRow row = ds.Tables[0].Rows[i];
                                        questionnaire.CreateBy = user;
                                        questionnaire.keyupload = row[0].ToString().Trim();
                                        if (DateTime.TryParse(row[1].ToString().Trim(), out DateTime sdate))
                                        {
                                            questionnaire.D_u_th_i_gian = sdate;
                                        }
                                        #region A.General information
                                        var A1__Product_origin = "";
                                        if (row["A1##Product#origin"].ToString().Trim() == "Imported products")
                                        {
                                            A1__Product_origin = "Sản phẩm nhập khẩu";
                                        }
                                        else if (row["A1##Product#origin"].ToString().Trim() == "Domestic products")
                                        {
                                            A1__Product_origin = "Sản phẩm sản xuất trong nước";
                                        }
                                        var A5__Type_of_product = "";
                                        if (row["A5##Type#of#product"].ToString().Trim() == "Powder (dạng bột)")
                                        {
                                            A5__Type_of_product = "Dạng bột";
                                        }
                                        else if (row["A5##Type#of#product"].ToString().Trim() == "Liquid (dạng dung dịch)")
                                        {
                                            A5__Type_of_product = "Dạng dung dịch";
                                        }
                                        var A6__Other_subtance_in_product = "";
                                        if (row["A6##Other#subtance#in#product"].ToString().Trim() == "No")
                                        {
                                            A6__Other_subtance_in_product = "Không";
                                        }
                                        else if (row["A6##Other#subtance#in#product"].ToString().Trim() == "Yes")
                                        {
                                            A6__Other_subtance_in_product = "Có";
                                        }
                                        var A8__Unit_of_volume_of_product = "";
                                        foreach (var item in lsunitvolumn)
                                        {
                                            if (row["A8##Unit#of#volume#of#product"].ToString().Trim() == item)
                                            {
                                                A8__Unit_of_volume_of_product = item;
                                            }
                                        }
                                        questionnaire.A1__Product_origin = A1__Product_origin;
                                        questionnaire.A2__Product_code = row["A2##Product#code"].ToString();
                                        questionnaire.A3__Product_name = row["A3##Product#name"].ToString();
                                        questionnaire.A4__Company = row["A4##Company"].ToString();
                                        questionnaire.A5__Type_of_product = A5__Type_of_product;
                                        questionnaire.A6__Other_subtance_in_product = A6__Other_subtance_in_product;
                                        questionnaire.A7__Volume_of_product = row["A7##Volume#of#product"].ToString();
                                        questionnaire.A8__Unit_of_volume_of_product = A8__Unit_of_volume_of_product;
                                        questionnaire.A9__Other_volume_of_product = row["A9##Other#volume#of#product"].ToString();
                                        #endregion

                                        #region B.Information related to antimicrobial
                                        var B2_3__Units_of_antimicrobial_1 = "";
                                        var B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_ = "";
                                        var B2_7__Units_of_product__link_to_question_B2_4_ = "";
                                        var B3_3__Units_of_antimicrobial_2 = "";
                                        var B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_ = "";
                                        var B3_7__Units_of_product__link_to_question_B3_4_ = "";
                                        var B4_3__Units_of_antimicrobial_3 = "";
                                        var B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_ = "";
                                        var B4_7__Units_of_product__link_to_question_B4_4_ = "";
                                        var B5_3__Units_of_antimicrobial_4 = "";
                                        var B5_5__Units_of_antimicrobial_4__link_to_question_5_4_ = "";
                                        var B5_7__Units_of_product__link_to_question_B5_4_ = "";
                                        foreach (var item in lsunitvolumn)
                                        {
                                            if (row["B2#3##Units#of#antimicrobial#1"].ToString().Trim() == item)
                                            {
                                                B2_3__Units_of_antimicrobial_1 = item;
                                            }
                                            if (row["B2#5##Units#of#antimicrobial#1##link#to#question#B2#4#"].ToString().Trim() == item)
                                            {
                                                B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_ = item;
                                            }
                                            if (row["B2#7##Units#of#product##link#to#question#B2#4#"].ToString().Trim() == item)
                                            {
                                                B2_7__Units_of_product__link_to_question_B2_4_ = item;
                                            }
                                            if (row["B3#3##Units#of#antimicrobial#2"].ToString().Trim() == item)
                                            {
                                                B3_3__Units_of_antimicrobial_2 = item;
                                            }
                                            if (row["B3#5##Units#of#antimicrobial#2##link#to#question#B3#4#"].ToString().Trim() == item)
                                            {
                                                B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_ = item;
                                            }
                                            if (row["B3#7##Units#of#product##link#to#question#B3#4#"].ToString().Trim() == item)
                                            {
                                                B3_7__Units_of_product__link_to_question_B3_4_ = item;
                                            }
                                            if (row["B4#3##Units#of#antimicrobial#3"].ToString().Trim() == item)
                                            {
                                                B4_3__Units_of_antimicrobial_3 = item;
                                            }
                                            if (row["B4#5##Units#of#antimicrobial#3##link#to#question#B4#4#"].ToString().Trim() == item)
                                            {
                                                B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_ = item;
                                            }
                                            if (row["B4#7##Units#of#product##link#to#question#B4#4#"].ToString().Trim() == item)
                                            {
                                                B4_7__Units_of_product__link_to_question_B4_4_ = item;
                                            }
                                            if (row["B5#3##Units#of#antimicrobial#4"].ToString().Trim() == item)
                                            {
                                                B5_3__Units_of_antimicrobial_4 = item;
                                            }
                                            if (row["B5#5##Units#of#antimicrobial#4##link#to#question#5#4#"].ToString().Trim() == item)
                                            {
                                                B5_5__Units_of_antimicrobial_4__link_to_question_5_4_ = item;
                                            }
                                            if (row["B5#7##Units#of#product##link#to#question#B5#4#"].ToString() == item)
                                            {
                                                B5_7__Units_of_product__link_to_question_B5_4_ = item;
                                            }

                                        }

                                        if (row["B1##Number#of#antimicrobials#in#product"].ToString().Trim() != "")
                                        {
                                            if (row["B1##Number#of#antimicrobials#in#product"].ToString().Trim() == "NA")
                                            {
                                                questionnaire.B1__Number_of_antimicrobials_in_product = -1;
                                            }
                                            else
                                            {
                                                questionnaire.B1__Number_of_antimicrobials_in_product = int.Parse(row["B1##Number#of#antimicrobials#in#product"].ToString());
                                            }
                                        }
                                        #region Antimicrobials 1 
                                        if(row["B2#1##Antimicrobial#1"].ToString().ToUpper().Trim() == "NA" || row["B2#1##Antimicrobial#1"].ToString().ToUpper().Trim() == "")
                                        {
                                            questionnaire.B2_1__Antimicrobial_1 = "NONE";
                                        }
                                        else if (row["B2#1##Antimicrobial#1"].ToString().ToUpper().Trim() == "L-CARNITINE")
                                        {
                                            questionnaire.B2_1__Antimicrobial_1 = "L_CARNITINE";
                                        }
                                        else
                                        {
                                            questionnaire.B2_1__Antimicrobial_1 = row["B2#1##Antimicrobial#1"].ToString().ToUpper().Trim();
                                        }
                                        if (row["B2#2##Strength#of#antimicrobial#1"].ToString() != "")
                                        {
                                            if(row["B2#2##Strength#of#antimicrobial#1"].ToString() == "NA")
                                            {
                                                questionnaire.B2_2__Strength_of_antimicrobial_1 = -1;
                                            }
                                            else
                                            {
                                                questionnaire.B2_2__Strength_of_antimicrobial_1 = double.Parse(row["B2#2##Strength#of#antimicrobial#1"].ToString());
                                            }
                                        }
                                        questionnaire.B2_3__Units_of_antimicrobial_1 = B2_3__Units_of_antimicrobial_1;
                                        questionnaire.B2_4__Per_amount_of_product__antimicrobial_1_ = row["B2#4##Per#amount#of#product##antimicrobial#1#"].ToString();
                                        questionnaire.B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_ = B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_;
                                        questionnaire.B2_6__Per_amount_of_product__volume_of_product___link_to_question_B2_4_ = row["B2#6##Per#amount#of#product##volume#of#product###link#to#questio"].ToString();
                                        questionnaire.B2_7__Units_of_product__link_to_question_B2_4_ = B2_7__Units_of_product__link_to_question_B2_4_;

                                        #endregion
                                        #region Antimicrobials 2
                                        if (row["B3#1##Antimicrobial#2"].ToString().ToUpper().Trim() == "NA" || row["B3#1##Antimicrobial#2"].ToString().ToUpper().Trim() == "")
                                        {
                                            questionnaire.B3_1__Antimicrobial_2 = "NONE";
                                        }
                                        else if (row["B3#1##Antimicrobial#2"].ToString().ToUpper().Trim() == "L-CARNITINE")
                                        {
                                            questionnaire.B3_1__Antimicrobial_2 = "L_CARNITINE";
                                        }
                                        else
                                        {
                                            questionnaire.B3_1__Antimicrobial_2 = row["B3#1##Antimicrobial#2"].ToString().ToUpper().Trim();
                                        }
                                        if (row["B3#2##Strength#of#antimicrobial#2"].ToString() != "")
                                        {
                                            if (row["B3#2##Strength#of#antimicrobial#2"].ToString() == "NA")
                                            {
                                                questionnaire.B3_2__Strength_of_antimicrobial_2 = -1;
                                            }
                                            else
                                            {
                                                questionnaire.B3_2__Strength_of_antimicrobial_2 = double.Parse(row["B3#2##Strength#of#antimicrobial#2"].ToString());
                                            }
                                        }
                                        questionnaire.B3_3__Units_of_antimicrobial_2 = B3_3__Units_of_antimicrobial_2;
                                        questionnaire.B3_4__Per_amount_of_product__antimicrobial_2_ = row["B3#4##Per#amount#of#product##antimicrobial#2#"].ToString();
                                        questionnaire.B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_ = B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_;
                                        questionnaire.B3_6__Per_amount_of_product__volume_of_product___link_to_question_B3_4_ = row["B3#6##Per#amount#of#product##volume#of#product###link#to#questio"].ToString();
                                        questionnaire.B3_7__Units_of_product__link_to_question_B3_4_ = B3_7__Units_of_product__link_to_question_B3_4_;
                                        #endregion
                                        #region Antimicrobials 3
                                        if (row["B4#1##Antimicrobial#3"].ToString().ToUpper().Trim() == "NA" || row["B4#1##Antimicrobial#3"].ToString().ToUpper().Trim() == "")
                                        {
                                            questionnaire.B4_1__Antimicrobial_3 = "NONE";
                                        }
                                        else if (row["B4#1##Antimicrobial#3"].ToString().ToUpper().Trim() == "L-CARNITINE")
                                        {
                                            questionnaire.B4_1__Antimicrobial_3 = "L_CARNITINE";
                                        }
                                        else
                                        {
                                            questionnaire.B4_1__Antimicrobial_3 = row["B4#1##Antimicrobial#3"].ToString().ToUpper().Trim();
                                        }
                                        var a = row["B3#2##Strength#of#antimicrobial#2"].ToString();
                                        if (row["B4#2##Strength#of#antimicrobial#3"].ToString() != "")
                                        {
                                            if (row["B4#2##Strength#of#antimicrobial#3"].ToString() == "NA")
                                            {
                                                questionnaire.B4_2__Strength_of_antimicrobial_3 = -1;
                                            }
                                            else
                                            {
                                                questionnaire.B4_2__Strength_of_antimicrobial_3 = double.Parse(row["B4#2##Strength#of#antimicrobial#3"].ToString());
                                            }
                                        }
                                        questionnaire.B4_3__Units_of_antimicrobial_3 = B4_3__Units_of_antimicrobial_3;
                                        questionnaire.B4_4__Per_amount_of_product__antimicrobial_3_ = row["B4#4##Per#amount#of#product##antimicrobial#3#"].ToString();
                                        questionnaire.B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_ = B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_;
                                        questionnaire.B4_6__Per_amount_of_product__volume_of_product___link_to_question_B4_4_ = row["B4#6##Per#amount#of#product##volume#of#product###link#to#questio"].ToString();
                                        questionnaire.B4_7__Units_of_product__link_to_question_B4_4_ = B4_7__Units_of_product__link_to_question_B4_4_;
                                        #endregion
                                        #region Antimicrobials 4
                                        if (row["B5#1##Antimicrobial#4"].ToString().ToUpper().Trim() == "NA" || row["B5#1##Antimicrobial#4"].ToString().ToUpper().Trim() == "")
                                        {
                                            questionnaire.B5_1__Antimicrobial_4 = "NONE";
                                        }
                                        else if (row["B5#1##Antimicrobial#4"].ToString().ToUpper().Trim() == "L-CARNITINE")
                                        {
                                            questionnaire.B5_1__Antimicrobial_4 = "L_CARNITINE";
                                        }
                                        else
                                        {
                                            questionnaire.B5_1__Antimicrobial_4 = row["B5#1##Antimicrobial#4"].ToString().ToUpper().Trim();
                                        }
                                        if (row["B5#2##Strength#of#antimicrobial#4"].ToString() != "" && row["B5#2##Strength#of#antimicrobial#4"].ToString() != "NA")
                                        {
                                            if (row["B5#2##Strength#of#antimicrobial#4"].ToString().Trim() == "NA")
                                            {
                                                questionnaire.B5_2__Strength_of_antimicrobial_4 = -1;
                                            }
                                            else
                                            {
                                                questionnaire.B5_2__Strength_of_antimicrobial_4 = double.Parse(row["B5#2##Strength#of#antimicrobial#4"].ToString());
                                            }
                                        }
                                        questionnaire.B5_3__Units_of_antimicrobial_4 = B5_3__Units_of_antimicrobial_4;
                                        questionnaire.B5_4__Per_amount_of_product__antimicrobial_4_ = row["B5#4##Per#amount#of#product##antimicrobial#4#"].ToString();
                                        questionnaire.B5_5__Units_of_antimicrobial_4__link_to_question_5_4_ = B5_5__Units_of_antimicrobial_4__link_to_question_5_4_;
                                        questionnaire.B5_6__Per_amount_of_product__volume_of_product___link_to_question_B5_4_ = row["B5#6##Per#amount#of#product##volume#of#product###link#to#questio"].ToString();
                                        questionnaire.B5_7__Units_of_product__link_to_question_B5_4_ = B5_7__Units_of_product__link_to_question_B5_4_;
                                        #endregion
                                        string[] lsPetDefault = { "BUFFALO", "CATTLE", "POULTRY", "PIG", "DOG", "CAT", "GOAT", "QUAIL", "SHEEP", "MUSCOVY_DUCK","DUCK", "GOOSE", "HORSE", "CHICKEN", "PIGLET", "CALF", "CHICK" };
                                        string[] lsRouteDefault = { "ORAL", "INJECTABLE", "WATER", "FEED" };
                                        var lsPetX = row["B6##Target#species"].ToString().Split(',').ToArray();
                                        List<string> lsPet = new List<string>();
                                        foreach (var item in lsPetX)
                                        {
                                            lsPet.Add(item.Trim());
                                        }
                                        var lsroute = row["B7##Administration#route"].ToString().Split(',').ToArray();
                                        var lsPetSelect = new List<Pet>(); var lsRouteSelect = new List<Route>();
                                        foreach (var item in lsPet)
                                        {
                                            var s = item.Trim().ToUpper();
                                            if (item != "" && lsPetDefault.Contains(s))
                                            {
                                                Pet pet = (Pet)Enum.Parse(typeof(Pet), s, true);
                                                if (System.Enum.IsDefined(typeof(Pet), pet))
                                                {
                                                    lsPetSelect.Add(pet);
                                                }
                                            }
                                        }
                                        foreach (var item in lsroute)
                                        {
                                            var s = item.Trim().ToUpper();
                                            if (item != "" && lsRouteDefault.Contains(s))
                                            {
                                                Route route = (Route)Enum.Parse(typeof(Route), s, true);
                                                if (System.Enum.IsDefined(typeof(Route), route))
                                                {
                                                    lsRouteSelect.Add(route);
                                                }
                                            }
                                        }
                                        var lspetsave = "";
                                        var lsroutesave = "";
                                        if (lsPetSelect != null)
                                        {
                                            var count = 0;
                                            foreach (var item in lsPetSelect)
                                            {
                                                count++;
                                                if (count == lsPetSelect.Count)
                                                {
                                                    lspetsave += item;
                                                }
                                                else
                                                {
                                                    lspetsave += item + ",";
                                                }
                                            }
                                        }
                                        if (lsRouteSelect != null)
                                        {
                                            var count = 0;
                                            foreach (var item in lsRouteSelect)
                                            {
                                                count++;
                                                if (count == lsRouteSelect.Count)
                                                {
                                                    lsroutesave += item;
                                                }
                                                else
                                                {
                                                    lsroutesave += item + ",";
                                                }
                                            }
                                        }
                                        questionnaire.B6__Target_species_x = lspetsave;
                                        questionnaire.B7__Administration_route = lsroutesave;
                                        #endregion

                                        #region C_ Heo
                                        var C1_2__Product_preparation_Unit_of_product = "";
                                        var C2_3__Unit_of_product = "";
                                        var C3_2__Product_preparation_Unit_of_product = "";
                                        var C4_3__Unit_of_product = "";
                                        foreach (var item in lsunitvolumn)
                                        {
                                            if (row["C1#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                            {
                                                C1_2__Product_preparation_Unit_of_product = item;
                                            }
                                            if (row["C2#3##Unit#of#product"].ToString().Trim() == item)
                                            {
                                                C2_3__Unit_of_product = item;
                                            }
                                            if (row["C3#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                            {
                                                C3_2__Product_preparation_Unit_of_product = item;
                                            }
                                            if (row["C4#3##Unit#of#product"].ToString().Trim() == item)
                                            {
                                                C4_3__Unit_of_product = item;
                                            }
                                        }
                                        #region C_1_ Product preparation (dilution) _pig_prevention purpose
                                        questionnaire.C1_1__Product_preparation__dilution__Product_amount = row["C1#1##Product#preparation##dilution#_Product#amount"].ToString();
                                        questionnaire.C1_2__Product_preparation_Unit_of_product = C1_2__Product_preparation_Unit_of_product;
                                        questionnaire.C1_3__Product_preparation_To_be_added_to__min_ = row["C1#3##Product#preparation_To#be#added#to##min#"].ToString();
                                        questionnaire.C1_4__Product_preparation_To_be_added_to__max_ = row["C1#4##Product#preparation_To#be#added#to##max#"].ToString();
                                        questionnaire.C1_5__Product_preparation_Unit_of_water_feed = row["C1#5##Product#preparation_Unit#of#water#feed"].ToString();
                                        questionnaire.C1_6__Duration_of_usage__min__max_ = row["C1#6##Duration#of#usage##min##max#"].ToString();
                                        #endregion

                                        #region C.2 Guidelines referred to bodyweight_pig_prevention purpose
                                        questionnaire.C2_1__Product_min = row["C2#1##Product#min"].ToString();
                                        questionnaire.C2_2__Product_max = row["C2#2##Product#max"].ToString();
                                        questionnaire.C2_3__Unit_of_product = C2_3__Unit_of_product;
                                        questionnaire.C2_4__Per_No__Kg_bodyweight_min = row["C2#4##Per#No##Kg#bodyweight_min"].ToString();
                                        questionnaire.C2_5__Per_No__Kg_bodyweight_max = row["C2#5##Per#No##Kg#bodyweight_max"].ToString();
                                        questionnaire.C2_6__Duration_of_usage = row["C2#6##Duration#of#usage"].ToString();
                                        #endregion

                                        #region C_3 Product preparation (dilution) _pig_treatment purpose
                                        questionnaire.C3_1__Product_preparation__dilution__Product_amount = row["C3#1##Product#preparation##dilution#_Product#amount"].ToString();
                                        questionnaire.C3_2__Product_preparation_Unit_of_product = C3_2__Product_preparation_Unit_of_product;
                                        questionnaire.C3_3__Product_preparation_To_be_added_to__min_ = row["C3#3##Product#preparation_To#be#added#to##min#"].ToString();
                                        questionnaire.C3_4__Product_preparation_To_be_added_to__max_ = row["C3#4##Product#preparation_To#be#added#to##max#"].ToString();
                                        questionnaire.C3_5__Product_preparation_Unit_of_water_feed = row["C3#5##Product#preparation_Unit#of#water#feed"].ToString();
                                        questionnaire.C3_6__Duration_of_usage = row["C3#6##Duration#of#usage"].ToString();
                                        #endregion

                                        #region C_4 Guidelines referred to bodyweight_pig_treatment purpose                  
                                        questionnaire.C4_1__Product_min = row["C4#1##Product#min"].ToString();
                                        questionnaire.C4_2__Product_max = row["C4#2##Product#max"].ToString();
                                        questionnaire.C4_3__Unit_of_product = C4_3__Unit_of_product;
                                        questionnaire.C4_4__Per_No__Kg_bodyweight_min = row["C4#4##Per#No##Kg#bodyweight_min"].ToString();
                                        questionnaire.C4_5__Per_No__Kg_bodyweight_max = row["C4#5##Per#No##Kg#bodyweight_max"].ToString();
                                        questionnaire.C4_6__Duration_of_usage = row["C4#6##Duration#of#usage"].ToString();
                                        #endregion

                                        #endregion

                                        #region D Động vật nhai lại
                                        var D1_2__Product_preparation_Unit_of_product = "";
                                        var D2_3__Unit_of_product = "";
                                        var D3_2__Product_preparation_Unit_of_product = "";
                                        var D4_3__Unit_of_product = "";
                                        foreach (var item in lsunitvolumn)
                                        {
                                            if (row["D1#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                            {
                                                D1_2__Product_preparation_Unit_of_product = item;
                                            }
                                            if (row["D2#3##Unit#of#product"].ToString().Trim() == item)
                                            {
                                                D2_3__Unit_of_product = item;
                                            }
                                            if (row["D3#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                            {
                                                D3_2__Product_preparation_Unit_of_product = item;
                                            }
                                            if (row["D4#3##Unit#of#product"].ToString().Trim() == item)
                                            {
                                                D4_3__Unit_of_product = item;
                                            }
                                        }
                                        #region C_1_ Product preparation (dilution) _pig_prevention purpose
                                        questionnaire.D1_1__Product_preparation__dilution__Product_amount = row["D1#1##Product#preparation##dilution#_Product#amount"].ToString();
                                        questionnaire.D1_2__Product_preparation_Unit_of_product = D1_2__Product_preparation_Unit_of_product;
                                        questionnaire.D1_3__Product_preparation_To_be_added_to__min_ = row["D1#3##Product#preparation_To#be#added#to##min#"].ToString();
                                        questionnaire.D1_4__Product_preparation_To_be_added_to__max_ = row["D1#4##Product#preparation_To#be#added#to##max#"].ToString();
                                        questionnaire.D1_5__Product_preparation_Unit_of_water_feed = row["D1#5##Product#preparation_Unit#of#water#feed"].ToString();
                                        questionnaire.D1_6__Duration_of_usage = row["D1#6##Duration#of#usage"].ToString();
                                        #endregion

                                        #region C.2 Guidelines referred to bodyweight_pig_prevention purpose
                                        questionnaire.D2_1__Product_min = row["D2#1##Product#min"].ToString();
                                        questionnaire.D2_2__Product_max = row["D2#2##Product#max"].ToString();
                                        questionnaire.D2_3__Unit_of_product = D2_3__Unit_of_product;
                                        questionnaire.D2_4__Per_No__Kg_bodyweight_min = row["D2#4##Per#No##Kg#bodyweight_min"].ToString();
                                        questionnaire.D2_5__Per_No__Kg_bodyweight_max = row["D2#5##Per#No##Kg#bodyweight_max"].ToString();
                                        questionnaire.D2_6__Duration_of_usage = row["D2#6##Duration#of#usage"].ToString();
                                        #endregion

                                        #region C_3 Product preparation (dilution) _pig_treatment purpose
                                        questionnaire.D3_1__Product_preparation__dilution__Product_amount = row["D3#1##Product#preparation##dilution#_Product#amount"].ToString();
                                        questionnaire.D3_2__Product_preparation_Unit_of_product = D3_2__Product_preparation_Unit_of_product;
                                        questionnaire.D3_3__Product_preparation_To_be_added_to__min_ = row["D3#3##Product#preparation_To#be#added#to##min#"].ToString();
                                        questionnaire.D3_4__Product_preparation_To_be_added_to__max_ = row["D3#4##Product#preparation_To#be#added#to##max#"].ToString();
                                        questionnaire.D3_5__Product_preparation_Unit_of_water_feed = row["D3#5##Product#preparation_Unit#of#water#feed"].ToString();
                                        questionnaire.D3_6__Duration_of_usage = row["D3#6##Duration#of#usage"].ToString();
                                        #endregion

                                        #region C_4 Guidelines referred to bodyweight_pig_treatment purpose                  
                                        questionnaire.D4_1__Product_min = row["D4#1##Product#min"].ToString();
                                        questionnaire.D4_2__Product_max = row["D4#2##Product#max"].ToString();
                                        questionnaire.D4_3__Unit_of_product = D4_3__Unit_of_product;
                                        questionnaire.D4_4__Per_No__Kg_bodyweight_min = row["D4#4##Per#No##Kg#bodyweight_min"].ToString();
                                        questionnaire.D4_5__Per_No__Kg_bodyweight_max = row["D4#5##Per#No##Kg#bodyweight_max"].ToString();
                                        questionnaire.D4_6__Duration_of_usage = row["D4#6##Duration#of#usage"].ToString();
                                        #endregion

                                        #endregion

                                        #region E. Gia cầm
                                        var E1_2__Product_preparation_Unit_of_product = "";
                                        var E2_3__Unit_of_product = "";
                                        var E3_2__Product_preparation_Unit_of_product = "";
                                        var E4_3__Unit_of_product = "";
                                        foreach (var item in lsunitvolumn)
                                        {
                                            if (row["E1#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                            {
                                                E1_2__Product_preparation_Unit_of_product = item;
                                            }
                                            if (row["E2#3##Unit#of#product"].ToString().Trim() == item)
                                            {
                                                E2_3__Unit_of_product = item;
                                            }
                                            if (row["E3#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                            {
                                                E3_2__Product_preparation_Unit_of_product = item;
                                            }
                                            if (row["E4#3##Unit#of#product"].ToString().Trim() == item)
                                            {
                                                E4_3__Unit_of_product = item;
                                            }
                                        }
                                        #region C_1_ Product preparation (dilution) _pig_prevention purpose
                                        questionnaire.E1_1__Product_preparation__dilution__Product_amount = row["E1#1##Product#preparation##dilution#_Product#amount"].ToString();
                                        questionnaire.E1_2__Product_preparation_Unit_of_product = E1_2__Product_preparation_Unit_of_product;
                                        questionnaire.E1_3__Product_preparation_To_be_added_to__min_ = row["E1#3##Product#preparation_To#be#added#to##min#"].ToString();
                                        questionnaire.E1_4__Product_preparation_To_be_added_to__max_ = row["E1#4##Product#preparation_To#be#added#to##max#"].ToString();
                                        questionnaire.E1_5__Product_preparation_Unit_of_water_feed = row["E1#5##Product#preparation_Unit#of#water#feed"].ToString();
                                        questionnaire.E1_6__Duration_of_usage = row["E1#6##Duration#of#usage"].ToString();
                                        #endregion

                                        #region C.2 Guidelines referred to bodyweight_pig_prevention purpose
                                        questionnaire.E2_1__Product_min = row["E2#1##Product#min"].ToString();
                                        questionnaire.E2_2__Product_max = row["E2#2##Product#max"].ToString();
                                        questionnaire.E2_3__Unit_of_product = E2_3__Unit_of_product;
                                        questionnaire.E2_4__Per_No__Kg_bodyweight_min = row["E2#4##Per#No##Kg#bodyweight_min"].ToString();
                                        questionnaire.E2_5__Per_No__Kg_bodyweight_max = row["E2#5##Per#No##Kg#bodyweight_max"].ToString();
                                        questionnaire.E2_6__Duration_of_usage = row["E2#6##Duration#of#usage"].ToString();
                                        #endregion

                                        #region C_3 Product preparation (dilution) _pig_treatment purpose
                                        questionnaire.E3_1__Product_preparation__dilution__Product_amount = row["E3#1##Product#preparation##dilution#_Product#amount"].ToString();
                                        questionnaire.E3_2__Product_preparation_Unit_of_product = E3_2__Product_preparation_Unit_of_product;
                                        questionnaire.E3_3__Product_preparation_To_be_added_to__min_ = row["E3#3##Product#preparation_To#be#added#to##min#"].ToString();
                                        questionnaire.E3_4__Product_preparation_To_be_added_to__max_ = row["E3#4##Product#preparation_To#be#added#to##max#"].ToString();
                                        questionnaire.E3_5__Product_preparation_Unit_of_water_feed = row["E3#5##Product#preparation_Unit#of#water#feed"].ToString();
                                        questionnaire.E3_6__Duration_of_usage = row["E3#6##Duration#of#usage"].ToString();
                                        #endregion

                                        #region C_4 Guidelines referred to bodyweight_pig_treatment purpose                  
                                        questionnaire.E4_1__Product_min = row["E4#1##Product#min"].ToString();
                                        questionnaire.E4_2__Product_max = row["E4#2##Product#max"].ToString();
                                        questionnaire.E4_3__Unit_of_product = E4_3__Unit_of_product;
                                        questionnaire.E4_4_Per_No__Kg_bodyweight_min = row["E4#4#Per#No##Kg#bodyweight_min"].ToString();
                                        questionnaire.E4_5__Per_No__Kg_bodyweight_max = row["E4#5##Per#No##Kg#bodyweight_max"].ToString();
                                        questionnaire.E4_6__Duration_of_usage = row["E4#6##Duration#of#usage"].ToString();
                                        #endregion

                                        #endregion

                                        #region F.Further information

                                        //questionnaire.F_1__Source_of_information = row["F#1##Source#of#information"].ToString();
                                        //questionnaire.F_2__Picture_of_product = row["F#2##Picture#of#product"].ToString();
                                        //questionnaire.F3__Correction = row["F3##Correction"].ToString();
                                        //questionnaire.F_4__Person_in_charge = row["F#4##Person#in#charge"].ToString();
                                        //questionnaire.F_5__Working_time = row["F#5##Working#time"].ToString();
                                        questionnaire.F_6__Note = row["F#6##Note#"].ToString();

                                        #endregion

                                        #region G.Piglet
                                        var Piglet_1_2__Product_preparation_Unit_of_product = "";
                                        var Piglet_2_3__Unit_of_product = "";
                                        var Piglet_3_2__Product_preparation_Unit_of_product = "";
                                        var Piglet_4_3__Unit_of_product = "";
                                        foreach (var item in lsunitvolumn)
                                        {
                                            if (row["Piglet#1#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                            {
                                                Piglet_1_2__Product_preparation_Unit_of_product = item;
                                            }
                                            if (row["Piglet#2#3##Unit#of#product"].ToString().Trim() == item)
                                            {
                                                Piglet_2_3__Unit_of_product = item;
                                            }
                                            if (row["Piglet#3#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                            {
                                                Piglet_3_2__Product_preparation_Unit_of_product = item;
                                            }
                                            if (row["Piglet#4#3##Unit#of#product"].ToString().Trim() == item)
                                            {
                                                Piglet_4_3__Unit_of_product = item;
                                            }
                                        }

                                        questionnaire.Piglet_1_1__Product_preparation__dilution__Product_amount = row["Piglet#1#1##Product#preparation##dilution#_Product#amount"].ToString();
                                        questionnaire.Piglet_1_2__Product_preparation_Unit_of_product = Piglet_1_2__Product_preparation_Unit_of_product;
                                        questionnaire.Piglet_1_3__Product_preparation_To_be_added_to__min_ = row["Piglet#1#3##Product#preparation_To#be#added#to##min#"].ToString();
                                        questionnaire.Piglet_1_4__Product_preparation_To_be_added_to__max_ = row["Piglet#1#4##Product#preparation_To#be#added#to##max#"].ToString();
                                        questionnaire.Piglet_1_5__Product_preparation_Unit_of_water_feed = row["Piglet#1#5##Product#preparation_Unit#of#water#feed"].ToString();
                                        questionnaire.Piglet_1_6__Duration_of_usage = row["Piglet#1#6##Duration#of#usage"].ToString();


                                        questionnaire.Piglet_2_1__Product_min = row["Piglet#2#1##Product#min"].ToString();
                                        questionnaire.Piglet_2_2__Product_max = row["Piglet#2#2##Product#max"].ToString();
                                        questionnaire.Piglet_2_3__Unit_of_product = Piglet_2_3__Unit_of_product;
                                        questionnaire.Piglet_2_4__Per_No__Kg_bodyweight_min = row["Piglet#2#4##Per#No##Kg#bodyweight_min"].ToString();
                                        questionnaire.Piglet_2_5__Per_No__Kg_bodyweight_max = row["Piglet#2#5##Per#No##Kg#bodyweight_max"].ToString();
                                        questionnaire.Piglet_2_6__Duration_of_usage = row["Piglet#2#6##Duration#of#usage"].ToString();


                                        questionnaire.Piglet_3_1__Product_preparation__dilution__Product_amount = row["Piglet#3#1##Product#preparation##dilution#_Product#amount"].ToString();
                                        questionnaire.Piglet_3_2__Product_preparation_Unit_of_product = Piglet_3_2__Product_preparation_Unit_of_product;
                                        questionnaire.Piglet_3_3__Product_preparation_To_be_added_to__min_ = row["Piglet#3#3##Product#preparation_To#be#added#to##min#"].ToString();
                                        questionnaire.Piglet_3_4__Product_preparation_To_be_added_to__max_ = row["Piglet#3#4##Product#preparation_To#be#added#to##max#"].ToString();
                                        questionnaire.Piglet_3_5__Product_preparation_Unit_of_water_feed = row["Piglet#3#5##Product#preparation_Unit#of#water#feed"].ToString();
                                        questionnaire.Piglet_3_6__Duration_of_usage = row["Piglet#3#6##Duration#of#usage"].ToString();



                                        questionnaire.Piglet_4_1__Product_min = row["Piglet#4#1##Product#min"].ToString();
                                        questionnaire.Piglet_4_2__Product_max = row["Piglet#4#2##Product#max"].ToString();
                                        questionnaire.Piglet_4_3__Unit_of_product = Piglet_4_3__Unit_of_product;
                                        questionnaire.Piglet_4_4_Per_No__Kg_bodyweight_min = row["Piglet#4#4#Per#No##Kg#bodyweight_min"].ToString();
                                        questionnaire.Piglet_4_5__Per_No__Kg_bodyweight_max = row["Piglet#4#5##Per#No##Kg#bodyweight_max"].ToString();
                                        questionnaire.Piglet_4_6__Duration_of_usage = row["Piglet#4#6##Duration#of#usage"].ToString();

                                        #endregion

                                        #region H.Buffalo
                                        var Buffalo_1_2__Product_preparation_Unit_of_product = "";
                                        var Buffalo_2_3__Unit_of_product = "";
                                        var Buffalo_3_2__Product_preparation_Unit_of_product = "";
                                        var Buffalo_4_3__Unit_of_product = "";
                                        var d = row["Buffalo#1#2##Product#preparation_Unit#of#product"].ToString();
                                        foreach (var item in lsunitvolumn)
                                        {
                                            if (row["Buffalo#1#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                            {
                                                Buffalo_1_2__Product_preparation_Unit_of_product = item;
                                            }
                                            if (row["Buffalo#2#3##Unit#of#product"].ToString().Trim() == item)
                                            {
                                                Buffalo_2_3__Unit_of_product = item;
                                            }
                                            if (row["Buffalo#3#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                            {
                                                Buffalo_3_2__Product_preparation_Unit_of_product = item;
                                            }
                                            if (row["Buffalo#4#3##Unit#of#product"].ToString().Trim() == item)
                                            {
                                                Buffalo_4_3__Unit_of_product = item;
                                            }
                                        }

                                        questionnaire.Buffalo_1_1__Product_preparation__dilution__Product_amount = row["Buffalo#1#1##Product#preparation##dilution#_Product#amount"].ToString();
                                        questionnaire.Buffalo_1_2__Product_preparation_Unit_of_product = Buffalo_1_2__Product_preparation_Unit_of_product;
                                        questionnaire.Buffalo_1_3__Product_preparation_To_be_added_to__min_ = row["Buffalo#1#3##Product#preparation_To#be#added#to##min#"].ToString();
                                        questionnaire.Buffalo_1_4__Product_preparation_To_be_added_to__max_ = row["Buffalo#1#4##Product#preparation_To#be#added#to##max#"].ToString();
                                        questionnaire.Buffalo_1_5__Product_preparation_Unit_of_water_feed = row["Buffalo#1#5##Product#preparation_Unit#of#water#feed"].ToString();
                                        questionnaire.Buffalo_1_6__Duration_of_usage = row["Buffalo#1#6##Duration#of#usage"].ToString();


                                        questionnaire.Buffalo_2_1__Product_min = row["Buffalo#2#1##Product#min"].ToString();
                                        questionnaire.Buffalo_2_2__Product_max = row["Buffalo#2#2##Product#max"].ToString();
                                        questionnaire.Buffalo_2_3__Unit_of_product = Buffalo_2_3__Unit_of_product;
                                        questionnaire.Buffalo_2_4__Per_No__Kg_bodyweight_min = row["Buffalo#2#4##Per#No##Kg#bodyweight_min"].ToString();
                                        questionnaire.Buffalo_2_5__Per_No__Kg_bodyweight_max = row["Buffalo#2#5##Per#No##Kg#bodyweight_max"].ToString();
                                        questionnaire.Buffalo_2_6__Duration_of_usage = row["Buffalo#2#6##Duration#of#usage"].ToString();


                                        questionnaire.Buffalo_3_1__Product_preparation__dilution__Product_amount = row["Buffalo#3#1##Product#preparation##dilution#_Product#amount"].ToString();
                                        questionnaire.Buffalo_3_2__Product_preparation_Unit_of_product = Buffalo_3_2__Product_preparation_Unit_of_product;
                                        questionnaire.Buffalo_3_3__Product_preparation_To_be_added_to__min_ = row["Buffalo#3#3##Product#preparation_To#be#added#to##min#"].ToString();
                                        questionnaire.Buffalo_3_4__Product_preparation_To_be_added_to__max_ = row["Buffalo#3#4##Product#preparation_To#be#added#to##max#"].ToString();
                                        questionnaire.Buffalo_3_5__Product_preparation_Unit_of_water_feed = row["Buffalo#3#5##Product#preparation_Unit#of#water#feed"].ToString();
                                        questionnaire.Buffalo_3_6__Duration_of_usage = row["Buffalo#3#6##Duration#of#usage"].ToString();



                                        questionnaire.Buffalo_4_1__Product_min = row["Buffalo#4#1##Product#min"].ToString();
                                        questionnaire.Buffalo_4_2__Product_max = row["Buffalo#4#2##Product#max"].ToString();
                                        questionnaire.Buffalo_4_3__Unit_of_product = Buffalo_4_3__Unit_of_product;
                                        questionnaire.Buffalo_4_4_Per_No__Kg_bodyweight_min = row["Buffalo#4#4#Per#No##Kg#bodyweight_min"].ToString();
                                        questionnaire.Buffalo_4_5__Per_No__Kg_bodyweight_max = row["Buffalo#4#5##Per#No##Kg#bodyweight_max"].ToString();
                                        questionnaire.Buffalo_4_6__Duration_of_usage = row["Buffalo#4#6##Duration#of#usage"].ToString();

                                        #endregion

                                        questionnaire.State = State.APPROVE;
                                        questionnaires.Add(questionnaire);
                                    }
                                }
                            }
                        }
                    }
                    
                }

                if (filePath.Length > 0)
                {
                    DataSet ds = new DataSet();

                    string ConnectionString = "";
                    if (filePath.EndsWith(".xls"))
                    {
                        ConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", filePath);
                    }
                    else if (filePath.EndsWith(".xlsx"))
                    {
                        ConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", filePath);
                    }
                    using (OleDbConnection conn = new System.Data.OleDb.OleDbConnection(ConnectionString))
                    {
                        conn.Open();
                        using (DataTable dtExcelSchema = conn.GetSchema("Tables"))
                        {
                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            string query1 = "SELECT * FROM [" + sheetName + "$IC1:QJ12000]";
                            OleDbDataAdapter adapter1 = new OleDbDataAdapter(query1, conn);
                            //DataSet ds = new DataSet();
                            adapter1.Fill(ds, "Items");
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)//ds.Tables[0].Rows.Count
                                    {
                                        DataRow row = ds.Tables[0].Rows[i];
                                        foreach (var questionnaire in questionnaires)
                                        {
                                            if (questionnaire.keyupload == (i + 1).ToString())
                                            {
                                                #region I.Cattle
                                                var Cattle_1_2__Product_preparation_Unit_of_product = "";
                                                var Cattle_2_3__Unit_of_product = "";
                                                var Cattle_3_2__Product_preparation_Unit_of_product = "";
                                                var Cattle_4_3__Unit_of_product = "";
                                                foreach (var item in lsunitvolumn)
                                                {
                                                    if (row["Cattle#1#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Cattle_1_2__Product_preparation_Unit_of_product = item;
                                                    }
                                                    if (row["Cattle#2#3##Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Cattle_2_3__Unit_of_product = item;
                                                    }
                                                    if (row["Cattle#3#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Cattle_3_2__Product_preparation_Unit_of_product = item;
                                                    }
                                                    if (row["Cattle#4#3##Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Cattle_4_3__Unit_of_product = item;
                                                    }
                                                }

                                                questionnaire.Cattle_1_1__Product_preparation__dilution__Product_amount = row["Cattle#1#1##Product#preparation##dilution#_Product#amount"].ToString();
                                                questionnaire.Cattle_1_2__Product_preparation_Unit_of_product = Cattle_1_2__Product_preparation_Unit_of_product;
                                                questionnaire.Cattle_1_3__Product_preparation_To_be_added_to__min_ = row["Cattle#1#3##Product#preparation_To#be#added#to##min#"].ToString();
                                                questionnaire.Cattle_1_4__Product_preparation_To_be_added_to__max_ = row["Cattle#1#4##Product#preparation_To#be#added#to##max#"].ToString();
                                                questionnaire.Cattle_1_5__Product_preparation_Unit_of_water_feed = row["Cattle#1#5##Product#preparation_Unit#of#water#feed"].ToString();
                                                questionnaire.Cattle_1_6__Duration_of_usages = row["Cattle#1#6##Duration#of#usage"].ToString();


                                                questionnaire.Cattle_2_1__Product_min = row["Cattle#2#1##Product#min"].ToString();
                                                questionnaire.Cattle_2_2__Product_max = row["Cattle#2#2##Product#max"].ToString();
                                                questionnaire.Cattle_2_3__Unit_of_product = Cattle_2_3__Unit_of_product;
                                                questionnaire.Cattle_2_4__Per_No__Kg_bodyweight_min = row["Cattle#2#4##Per#No##Kg#bodyweight_min"].ToString();
                                                questionnaire.Cattle_2_5__Per_No__Kg_bodyweight_max = row["Cattle#2#5##Per#No##Kg#bodyweight_max"].ToString();
                                                questionnaire.Cattle_2_6__Duration_of_usage = row["Cattle#2#6##Duration#of#usage"].ToString();


                                                questionnaire.Cattle_3_1__Product_preparation__dilution__Product_amount = row["Cattle#3#1##Product#preparation##dilution#_Product#amount"].ToString();
                                                questionnaire.Cattle_3_2__Product_preparation_Unit_of_product = Cattle_3_2__Product_preparation_Unit_of_product;
                                                questionnaire.Cattle_3_3__Product_preparation_To_be_added_to__min_ = row["Cattle#3#3##Product#preparation_To#be#added#to##min#"].ToString();
                                                questionnaire.Cattle_3_4__Product_preparation_To_be_added_to__max_ = row["Cattle#3#4##Product#preparation_To#be#added#to##max#"].ToString();
                                                questionnaire.Cattle_3_5__Product_preparation_Unit_of_water_feed = row["Cattle#3#5##Product#preparation_Unit#of#water#feed"].ToString();
                                                questionnaire.Cattle_3_6__Duration_of_usage = row["Cattle#3#6##Duration#of#usage"].ToString();



                                                questionnaire.Cattle_4_1__Product_min = row["Cattle#4#1##Product#min"].ToString();
                                                questionnaire.Cattle_4_2__Product_max = row["Cattle#4#2##Product#max"].ToString();
                                                questionnaire.Cattle_4_3__Unit_of_product = Cattle_4_3__Unit_of_product;
                                                questionnaire.Cattle_4_4_Per_No__Kg_bodyweight_min = row["Cattle#4#4#Per#No##Kg#bodyweight_min"].ToString();
                                                questionnaire.Cattle_4_5__Per_No__Kg_bodyweight_max = row["Cattle#4#5##Per#No##Kg#bodyweight_max"].ToString();
                                                questionnaire.Cattle_4_6__Duration_of_usage = row["Cattle#4#6##Duration#of#usage"].ToString();

                                                #endregion

                                                #region J.Goat
                                                var Goat_1_2__Product_preparation_Unit_of_product = "";
                                                var Goat_2_3__Unit_of_product = "";
                                                var Goat_3_2__Product_preparation_Unit_of_product = "";
                                                var Goat_4_3__Unit_of_product = "";
                                                foreach (var item in lsunitvolumn)
                                                {
                                                    if (row["Goat#1#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Goat_1_2__Product_preparation_Unit_of_product = item;
                                                    }
                                                    if (row["Goat#2#3##Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Goat_2_3__Unit_of_product = item;
                                                    }
                                                    if (row["Goat#3#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Goat_3_2__Product_preparation_Unit_of_product = item;
                                                    }
                                                    if (row["Goat#4#3##Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Goat_4_3__Unit_of_product = item;
                                                    }
                                                }
                                                questionnaire.Goat_1_1__Product_preparation__dilution__Product_amount = row["Goat#1#1##Product#preparation##dilution#_Product#amount"].ToString();
                                                questionnaire.Goat_1_2__Product_preparation_Unit_of_product = Goat_1_2__Product_preparation_Unit_of_product;
                                                questionnaire.Goat_1_3__Product_preparation_To_be_added_to__min_ = row["Goat#1#3##Product#preparation_To#be#added#to##min#"].ToString();
                                                questionnaire.Goat_1_4__Product_preparation_To_be_added_to__max_ = row["Goat#1#4##Product#preparation_To#be#added#to##max#"].ToString();
                                                questionnaire.Goat_1_5__Product_preparation_Unit_of_water_feed = row["Goat#1#5##Product#preparation_Unit#of#water#feed"].ToString();
                                                questionnaire.Goat_1_6__Duration_of_usage = row["Goat#1#6##Duration#of#usage"].ToString();


                                                questionnaire.Goat_2_1__Product_min = row["Goat#2#1##Product#min"].ToString();
                                                questionnaire.Goat_2_2__Product_max = row["Goat#2#2##Product#max"].ToString();
                                                questionnaire.Goat_2_3__Unit_of_product = Goat_2_3__Unit_of_product;
                                                questionnaire.Goat_2_4__Per_No__Kg_bodyweight_min = row["Goat#2#4##Per#No##Kg#bodyweight_min"].ToString();
                                                questionnaire.Goat_2_5__Per_No__Kg_bodyweight_max = row["Goat#2#5##Per#No##Kg#bodyweight_max"].ToString();
                                                questionnaire.Goat_2_6__Duration_of_usage = row["Goat#2#6##Duration#of#usage"].ToString();


                                                questionnaire.Goat_3_1__Product_preparation__dilution__Product_amount = row["Goat#3#1##Product#preparation##dilution#_Product#amount"].ToString();
                                                questionnaire.Goat_3_2__Product_preparation_Unit_of_product = Goat_3_2__Product_preparation_Unit_of_product;
                                                questionnaire.Goat_3_3__Product_preparation_To_be_added_to__min_ = row["Goat#3#3##Product#preparation_To#be#added#to##min#"].ToString();
                                                questionnaire.Goat_3_4__Product_preparation_To_be_added_to__max_ = row["Goat#3#4##Product#preparation_To#be#added#to##max#"].ToString();
                                                questionnaire.Goat_3_5__Product_preparation_Unit_of_water_feed = row["Goat#3#5##Product#preparation_Unit#of#water#feed"].ToString();
                                                questionnaire.Goat_3_6__Duration_of_usage = row["Goat#3#6##Duration#of#usage"].ToString();



                                                questionnaire.Goat_4_1__Product_min = row["Goat#4#1##Product#min"].ToString();
                                                questionnaire.Goat_4_2__Product_max = row["Goat#4#2##Product#max"].ToString();
                                                questionnaire.Goat_4_3__Unit_of_product = Goat_4_3__Unit_of_product;
                                                questionnaire.Goat_4_4_Per_No__Kg_bodyweight_min = row["Goat#4#4#Per#No##Kg#bodyweight_min"].ToString();
                                                questionnaire.Goat_4_5__Per_No__Kg_bodyweight_max = row["Goat#4#5##Per#No##Kg#bodyweight_max"].ToString();
                                                questionnaire.Goat_4_6__Duration_of_usage = row["Goat#4#6##Duration#of#usage"].ToString();

                                                #endregion

                                                #region K.Sheep
                                                var Sheep_1_2__Product_preparation_Unit_of_product = "";
                                                var Sheep_2_3__Unit_of_product = "";
                                                var Sheep_3_2__Product_preparation_Unit_of_product = "";
                                                var Sheep_4_3__Unit_of_product = "";
                                                foreach (var item in lsunitvolumn)
                                                {
                                                    if (row["Sheep#1#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Sheep_1_2__Product_preparation_Unit_of_product = item;
                                                    }
                                                    if (row["Sheep#2#3##Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Sheep_2_3__Unit_of_product = item;
                                                    }
                                                    if (row["Sheep#3#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Sheep_3_2__Product_preparation_Unit_of_product = item;
                                                    }
                                                    if (row["Sheep#4#3##Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Sheep_4_3__Unit_of_product = item;
                                                    }
                                                }
                                                questionnaire.Sheep_1_1__Product_preparation__dilution__Product_amount = row["Sheep#1#1##Product#preparation##dilution#_Product#amount"].ToString();
                                                questionnaire.Sheep_1_2__Product_preparation_Unit_of_product = Sheep_1_2__Product_preparation_Unit_of_product;
                                                questionnaire.Sheep_1_3__Product_preparation_To_be_added_to__min_ = row["Sheep#1#3##Product#preparation_To#be#added#to##min#"].ToString();
                                                questionnaire.Sheep_1_4__Product_preparation_To_be_added_to__max_ = row["Sheep#1#4##Product#preparation_To#be#added#to##max#"].ToString();
                                                questionnaire.Sheep_1_5__Product_preparation_Unit_of_water_feed = row["Sheep#1#5##Product#preparation_Unit#of#water#feed"].ToString();
                                                questionnaire.Sheep_1_6__Duration_of_usage = row["Sheep#1#6##Duration#of#usage"].ToString();


                                                questionnaire.Sheep_2_1__Product_min = row["Sheep#2#1##Product#min"].ToString();
                                                questionnaire.Sheep_2_2__Product_max = row["Sheep#2#2##Product#max"].ToString();
                                                questionnaire.Sheep_2_3__Unit_of_product = Sheep_2_3__Unit_of_product;
                                                questionnaire.Sheep_2_4__Per_No__Kg_bodyweight_min = row["Sheep#2#4##Per#No##Kg#bodyweight_min"].ToString();
                                                questionnaire.Sheep_2_5__Per_No__Kg_bodyweight_max = row["Sheep#2#5##Per#No##Kg#bodyweight_max"].ToString();
                                                questionnaire.Sheep_2_6__Duration_of_usage = row["Sheep#2#6##Duration#of#usage"].ToString();


                                                questionnaire.Sheep_3_1__Product_preparation__dilution__Product_amount = row["Sheep#3#1##Product#preparation##dilution#_Product#amount"].ToString();
                                                questionnaire.Sheep_3_2__Product_preparation_Unit_of_product = Sheep_3_2__Product_preparation_Unit_of_product;
                                                questionnaire.Sheep_3_3__Product_preparation_To_be_added_to__min_ = row["Sheep#3#3##Product#preparation_To#be#added#to##min#"].ToString();
                                                questionnaire.Sheep_3_4__Product_preparation_To_be_added_to__max_ = row["Sheep#3#4##Product#preparation_To#be#added#to##max#"].ToString();
                                                questionnaire.Sheep_3_5__Product_preparation_Unit_of_water_feed = row["Sheep#3#5##Product#preparation_Unit#of#water#feed"].ToString();
                                                questionnaire.Sheep_3_6__Duration_of_usage = row["Sheep#3#6##Duration#of#usage"].ToString();



                                                questionnaire.Sheep_4_1__Product_min = row["Sheep#4#1##Product#min"].ToString();
                                                questionnaire.Sheep_4_2__Product_max = row["Sheep#4#2##Product#max"].ToString();
                                                questionnaire.Sheep_4_3__Unit_of_product = Sheep_4_3__Unit_of_product;
                                                questionnaire.Sheep_4_4_Per_No__Kg_bodyweight_min = row["Sheep#4#4#Per#No##Kg#bodyweight_min"].ToString();
                                                questionnaire.Sheep_4_5__Per_No__Kg_bodyweight_max = row["Sheep#4#5##Per#No##Kg#bodyweight_max"].ToString();
                                                questionnaire.Sheep_4_6__Duration_of_usage = row["Sheep#4#6##Duration#of#usage"].ToString();

                                                #endregion

                                                #region L.Horse
                                                var Horse_1_2__Product_preparation_Unit_of_product = "";
                                                var Horse_2_3__Unit_of_product = "";
                                                var Horse_3_2__Product_preparation_Unit_of_product = "";
                                                var Horse_4_3__Unit_of_product = "";
                                                foreach (var item in lsunitvolumn)
                                                {
                                                    if (row["Horse#1#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Horse_1_2__Product_preparation_Unit_of_product = item;
                                                    }
                                                    if (row["Horse#2#3##Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Horse_2_3__Unit_of_product = item;
                                                    }
                                                    if (row["Horse#3#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Horse_3_2__Product_preparation_Unit_of_product = item;
                                                    }
                                                    if (row["Horse#4#3##Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Horse_4_3__Unit_of_product = item;
                                                    }
                                                }
                                                questionnaire.Horse_1_1__Product_preparation__dilution__Product_amount = row["Horse#1#1##Product#preparation##dilution#_Product#amount"].ToString();
                                                questionnaire.Horse_1_2__Product_preparation_Unit_of_product = Horse_1_2__Product_preparation_Unit_of_product;
                                                questionnaire.Horse_1_3__Product_preparation_To_be_added_to__min_ = row["Horse#1#3##Product#preparation_To#be#added#to##min#"].ToString();
                                                questionnaire.Horse_1_4__Product_preparation_To_be_added_to__max_ = row["Horse#1#4##Product#preparation_To#be#added#to##max#"].ToString();
                                                questionnaire.Horse_1_5__Product_preparation_Unit_of_water_feed = row["Horse#1#5##Product#preparation_Unit#of#water#feed"].ToString();
                                                questionnaire.Horse_1_6__Duration_of_usage = row["Horse#1#6##Duration#of#usage"].ToString();


                                                questionnaire.Horse_2_1__Product_min = row["Horse#2#1##Product#min"].ToString();
                                                questionnaire.Horse_2_2__Product_max = row["Horse#2#2##Product#max"].ToString();
                                                questionnaire.Horse_2_3__Unit_of_product = Horse_2_3__Unit_of_product;
                                                questionnaire.Horse_2_4__Per_No__Kg_bodyweight_min = row["Horse#2#4##Per#No##Kg#bodyweight_min"].ToString();
                                                questionnaire.Horse_2_5__Per_No__Kg_bodyweight_max = row["Horse#2#5##Per#No##Kg#bodyweight_max"].ToString();
                                                questionnaire.Horse_2_6__Duration_of_usage = row["Horse#2#6##Duration#of#usage"].ToString();


                                                questionnaire.Horse_3_1__Product_preparation__dilution__Product_amount = row["Horse#3#1##Product#preparation##dilution#_Product#amount"].ToString();
                                                questionnaire.Horse_3_2__Product_preparation_Unit_of_product = Horse_3_2__Product_preparation_Unit_of_product;
                                                questionnaire.Horse_3_3__Product_preparation_To_be_added_to__min_ = row["Horse#3#3##Product#preparation_To#be#added#to##min#"].ToString();
                                                questionnaire.Horse_3_4__Product_preparation_To_be_added_to__max_ = row["Horse#3#4##Product#preparation_To#be#added#to##max#"].ToString();
                                                questionnaire.Horse_3_5__Product_preparation_Unit_of_water_feed = row["Horse#3#5##Product#preparation_Unit#of#water#feed"].ToString();
                                                questionnaire.Horse_3_6__Duration_of_usage = row["Horse#3#6##Duration#of#usage"].ToString();



                                                questionnaire.Horse_4_1__Product_min = row["Horse#4#1##Product#min"].ToString();
                                                questionnaire.Horse_4_2__Product_max = row["Horse#4#2##Product#max"].ToString();
                                                questionnaire.Horse_4_3__Unit_of_product = Horse_4_3__Unit_of_product;
                                                questionnaire.Horse_4_4_Per_No__Kg_bodyweight_min = row["Horse#4#4#Per#No##Kg#bodyweight_min"].ToString();
                                                questionnaire.Horse_4_5__Per_No__Kg_bodyweight_max = row["Horse#4#5##Per#No##Kg#bodyweight_max"].ToString();
                                                questionnaire.Horse_4_6__Duration_of_usage = row["Horse#4#6##Duration#of#usage"].ToString();

                                                #endregion

                                                #region M.Chicken
                                                var Chicken_1_2__Product_preparation_Unit_of_product = "";
                                                var Chicken_2_3__Unit_of_product = "";
                                                var Chicken_3_2__Product_preparation_Unit_of_product = "";
                                                var Chicken_4_3__Unit_of_product = "";
                                                foreach (var item in lsunitvolumn)
                                                {
                                                    if (row["Chicken#1#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Chicken_1_2__Product_preparation_Unit_of_product = item;
                                                    }
                                                    if (row["Chicken#2#3##Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Chicken_2_3__Unit_of_product = item;
                                                    }
                                                    if (row["Chicken#3#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Chicken_3_2__Product_preparation_Unit_of_product = item;
                                                    }
                                                    if (row["Chicken#4#3##Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Chicken_4_3__Unit_of_product = item;
                                                    }
                                                }
                                                questionnaire.Chicken_1_1__Product_preparation__dilution__Product_amount = row["Chicken#1#1##Product#preparation##dilution#_Product#amount"].ToString();
                                                questionnaire.Chicken_1_2__Product_preparation_Unit_of_product = Chicken_1_2__Product_preparation_Unit_of_product;
                                                questionnaire.Chicken_1_3__Product_preparation_To_be_added_to__min_ = row["Chicken#1#3##Product#preparation_To#be#added#to##min#"].ToString();
                                                questionnaire.Chicken_1_4__Product_preparation_To_be_added_to__max_ = row["Chicken#1#4##Product#preparation_To#be#added#to##max#"].ToString();
                                                questionnaire.Chicken_1_5__Product_preparation_Unit_of_water_feed = row["Chicken#1#5##Product#preparation_Unit#of#water#feed"].ToString();
                                                questionnaire.Chicken_1_6__Duration_of_usage = row["Chicken#1#6##Duration#of#usage"].ToString();


                                                questionnaire.Chicken_2_1__Product_min = row["Chicken#2#1##Product#min"].ToString();
                                                questionnaire.Chicken_2_2__Product_max = row["Chicken#2#2##Product#max"].ToString();
                                                questionnaire.Chicken_2_3__Unit_of_product = Chicken_2_3__Unit_of_product;
                                                questionnaire.Chicken_2_4__Per_No__Kg_bodyweight_min = row["Chicken#2#4##Per#No##Kg#bodyweight_min"].ToString();
                                                questionnaire.Chicken_2_5__Per_No__Kg_bodyweight_max = row["Chicken#2#5##Per#No##Kg#bodyweight_max"].ToString();
                                                questionnaire.Chicken_2_6__Duration_of_usage = row["Chicken#2#6##Duration#of#usage"].ToString();


                                                questionnaire.Chicken_3_1__Product_preparation__dilution__Product_amount = row["Chicken#3#1##Product#preparation##dilution#_Product#amount"].ToString();
                                                questionnaire.Chicken_3_2__Product_preparation_Unit_of_product = Chicken_3_2__Product_preparation_Unit_of_product;
                                                questionnaire.Chicken_3_3__Product_preparation_To_be_added_to__min_ = row["Chicken#3#3##Product#preparation_To#be#added#to##min#"].ToString();
                                                questionnaire.Chicken_3_4__Product_preparation_To_be_added_to__max_ = row["Chicken#3#4##Product#preparation_To#be#added#to##max#"].ToString();
                                                questionnaire.Chicken_3_5__Product_preparation_Unit_of_water_feed = row["Chicken#3#5##Product#preparation_Unit#of#water#feed"].ToString();
                                                questionnaire.Chicken_3_6__Duration_of_usage = row["Chicken#3#6##Duration#of#usage"].ToString();



                                                questionnaire.Chicken_4_1__Product_min = row["Chicken#4#1##Product#min"].ToString();
                                                questionnaire.Chicken_4_2__Product_max = row["Chicken#4#2##Product#max"].ToString();
                                                questionnaire.Chicken_4_3__Unit_of_product = Chicken_4_3__Unit_of_product;
                                                questionnaire.Chicken_4_4_Per_No__Kg_bodyweight_min = row["Chicken#4#4#Per#No##Kg#bodyweight_min"].ToString();
                                                questionnaire.Chicken_4_5__Per_No__Kg_bodyweight_max = row["Chicken#4#5##Per#No##Kg#bodyweight_max"].ToString();
                                                questionnaire.Chicken_4_6__Duration_of_usage = row["Chicken#4#6##Duration#of#usage"].ToString();

                                                #endregion

                                                #region N.Duck
                                                var Duck_1_2__Product_preparation_Unit_of_product = "";
                                                var Duck_2_3__Unit_of_product = "";
                                                var Duck_3_2__Product_preparation_Unit_of_product = "";
                                                var Duck_4_3__Unit_of_product = "";
                                                foreach (var item in lsunitvolumn)
                                                {
                                                    if (row["Duck#1#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Duck_1_2__Product_preparation_Unit_of_product = item;
                                                    }
                                                    if (row["Duck#2#3##Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Duck_2_3__Unit_of_product = item;
                                                    }
                                                    if (row["Duck#3#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Duck_3_2__Product_preparation_Unit_of_product = item;
                                                    }
                                                    if (row["Duck#4#3##Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Duck_4_3__Unit_of_product = item;
                                                    }
                                                }

                                                questionnaire.Duck_1_1__Product_preparation__dilution__Product_amount = row["Duck#1#1##Product#preparation##dilution#_Product#amount"].ToString();
                                                questionnaire.Duck_1_2__Product_preparation_Unit_of_product = Duck_1_2__Product_preparation_Unit_of_product;
                                                questionnaire.Duck_1_3__Product_preparation_To_be_added_to__min_ = row["Duck#1#3##Product#preparation_To#be#added#to##min#"].ToString();
                                                questionnaire.Duck_1_4__Product_preparation_To_be_added_to__max_ = row["Duck#1#4##Product#preparation_To#be#added#to##max#"].ToString();
                                                questionnaire.Duck_1_5__Product_preparation_Unit_of_water_feed = row["Duck#1#5##Product#preparation_Unit#of#water#feed"].ToString();
                                                questionnaire.Duck_1_6__Duration_of_usage = row["Duck#1#6##Duration#of#usage"].ToString();


                                                questionnaire.Duck_2_1__Product_min = row["Duck#2#1##Product#min"].ToString();
                                                questionnaire.Duck_2_2__Product_max = row["Duck#2#2##Product#max"].ToString();
                                                questionnaire.Duck_2_3__Unit_of_product = Duck_2_3__Unit_of_product;
                                                questionnaire.Duck_2_4__Per_No__Kg_bodyweight_min = row["Duck#2#4##Per#No##Kg#bodyweight_min"].ToString();
                                                questionnaire.Duck_2_5__Per_No__Kg_bodyweight_max = row["Duck#2#5##Per#No##Kg#bodyweight_max"].ToString();
                                                questionnaire.Duck_2_6__Duration_of_usage = row["Duck#2#6##Duration#of#usage"].ToString();


                                                questionnaire.Duck_3_1__Product_preparation__dilution__Product_amount = row["Duck#3#1##Product#preparation##dilution#_Product#amount"].ToString();
                                                questionnaire.Duck_3_2__Product_preparation_Unit_of_product = Duck_3_2__Product_preparation_Unit_of_product;
                                                questionnaire.Duck_3_3__Product_preparation_To_be_added_to__min_ = row["Duck#3#3##Product#preparation_To#be#added#to##min#"].ToString();
                                                questionnaire.Duck_3_4__Product_preparation_To_be_added_to__max_ = row["Duck#3#4##Product#preparation_To#be#added#to##max#"].ToString();
                                                questionnaire.Duck_3_5__Product_preparation_Unit_of_water_feed = row["Duck#3#5##Product#preparation_Unit#of#water#feed"].ToString();
                                                questionnaire.Duck_3_6__Duration_of_usage = row["Duck#3#6##Duration#of#usage"].ToString();



                                                questionnaire.Duck_4_1__Product_min = row["Duck#4#1##Product#min"].ToString();
                                                questionnaire.Duck_4_2__Product_max = row["Duck#4#2##Product#max"].ToString();
                                                questionnaire.Duck_4_3__Unit_of_product = Duck_4_3__Unit_of_product;
                                                questionnaire.Duck_4_4_Per_No__Kg_bodyweight_min = row["Duck#4#4#Per#No##Kg#bodyweight_min"].ToString();
                                                questionnaire.Duck_4_5__Per_No__Kg_bodyweight_max = row["Duck#4#5##Per#No##Kg#bodyweight_max"].ToString();
                                                questionnaire.Duck_4_6__Duration_of_usage = row["Duck#4#6##Duration#of#usage"].ToString();

                                                #endregion

                                                #region O.Muscovy_Duck
                                                var Muscovy_Duck_1_2__Product_preparation_Unit_of_product = "";
                                                var Muscovy_Duck_2_3__Unit_of_product = "";
                                                var Muscovy_Duck_3_2__Product_preparation_Unit_of_product = "";
                                                var Muscovy_Duck_4_3__Unit_of_product = "";
                                                foreach (var item in lsunitvolumn)
                                                {
                                                    if (row["Muscovy#Duck#1#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Muscovy_Duck_1_2__Product_preparation_Unit_of_product = item;
                                                    }
                                                    if (row["Muscovy#Duck#2#3##Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Muscovy_Duck_2_3__Unit_of_product = item;
                                                    }
                                                    if (row["Muscovy#Duck#3#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Muscovy_Duck_3_2__Product_preparation_Unit_of_product = item;
                                                    }
                                                    if (row["Muscovy#Duck#4#3##Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Muscovy_Duck_4_3__Unit_of_product = item;
                                                    }
                                                }

                                                questionnaire.Muscovy_Duck_1_1__Product_preparation__dilution__Product_amount = row["Muscovy#Duck#1#1##Product#preparation##dilution#_Product#amount"].ToString();
                                                questionnaire.Muscovy_Duck_1_2__Product_preparation_Unit_of_product = Muscovy_Duck_1_2__Product_preparation_Unit_of_product;
                                                questionnaire.Muscovy_Duck_1_3__Product_preparation_To_be_added_to__min_ = row["Muscovy#Duck#1#3##Product#preparation_To#be#added#to##min#"].ToString();
                                                questionnaire.Muscovy_Duck_1_4__Product_preparation_To_be_added_to__max_ = row["Muscovy#Duck#1#4##Product#preparation_To#be#added#to##max#"].ToString();
                                                questionnaire.Muscovy_Duck_1_5__Product_preparation_Unit_of_water_feed = row["Muscovy#Duck#1#5##Product#preparation_Unit#of#water#feed"].ToString();
                                                questionnaire.Muscovy_Duck_1_6__Duration_of_usage = row["Muscovy#Duck#1#6##Duration#of#usage"].ToString();


                                                questionnaire.Muscovy_Duck_2_1__Product_min = row["Muscovy#Duck#2#1##Product#min"].ToString();
                                                questionnaire.Muscovy_Duck_2_2__Product_max = row["Muscovy#Duck#2#2##Product#max"].ToString();
                                                questionnaire.Muscovy_Duck_2_3__Unit_of_product = Muscovy_Duck_2_3__Unit_of_product;
                                                questionnaire.Muscovy_Duck_2_4__Per_No__Kg_bodyweight_min = row["Muscovy#Duck#2#4##Per#No##Kg#bodyweight_min"].ToString();
                                                questionnaire.Muscovy_Duck_2_5__Per_No__Kg_bodyweight_max = row["Muscovy#Duck#2#5##Per#No##Kg#bodyweight_max"].ToString();
                                                questionnaire.Muscovy_Duck_2_6__Duration_of_usage = row["Muscovy#Duck#2#6##Duration#of#usage"].ToString();


                                                questionnaire.Muscovy_Duck_3_1__Product_preparation__dilution__Product_amount = row["Muscovy#Duck#3#1##Product#preparation##dilution#_Product#amount"].ToString();
                                                questionnaire.Muscovy_Duck_3_2__Product_preparation_Unit_of_product = Muscovy_Duck_3_2__Product_preparation_Unit_of_product;
                                                questionnaire.Muscovy_Duck_3_3__Product_preparation_To_be_added_to__min_ = row["Muscovy#Duck#3#3##Product#preparation_To#be#added#to##min#"].ToString();
                                                questionnaire.Muscovy_Duck_3_4__Product_preparation_To_be_added_to__max_ = row["Muscovy#Duck#3#4##Product#preparation_To#be#added#to##max#"].ToString();
                                                questionnaire.Muscovy_Duck_3_5__Product_preparation_Unit_of_water_feed = row["Muscovy#Duck#3#5##Product#preparation_Unit#of#water#feed"].ToString();
                                                questionnaire.Muscovy_Duck_3_6__Duration_of_usage = row["Muscovy#Duck#3#6##Duration#of#usage"].ToString();



                                                questionnaire.Muscovy_Duck_4_1__Product_min = row["Muscovy#Duck#4#1##Product#min"].ToString();
                                                questionnaire.Muscovy_Duck_4_2__Product_max = row["Muscovy#Duck#4#2##Product#max"].ToString();
                                                questionnaire.Muscovy_Duck_4_3__Unit_of_product = Muscovy_Duck_4_3__Unit_of_product;
                                                questionnaire.Muscovy_Duck_4_4_Per_No__Kg_bodyweight_min = row["Muscovy#Duck#4#4#Per#No##Kg#bodyweight_min"].ToString();
                                                questionnaire.Muscovy_Duck_4_5__Per_No__Kg_bodyweight_max = row["Muscovy#Duck#4#5##Per#No##Kg#bodyweight_max"].ToString();
                                                questionnaire.Muscovy_Duck_4_6__Duration_of_usage = row["Muscovy#Duck#4#6##Duration#of#usage"].ToString();

                                                #endregion

                                                #region P.Quail
                                                var Quail_1_2__Product_preparation_Unit_of_product = "";
                                                var Quail_2_3__Unit_of_product = "";
                                                var Quail_3_2__Product_preparation_Unit_of_product = "";
                                                var Quail_4_3__Unit_of_product = "";
                                                foreach (var item in lsunitvolumn)
                                                {
                                                    if (row["Quail#1#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Quail_1_2__Product_preparation_Unit_of_product = item;
                                                    }
                                                    if (row["Quail#2#3##Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Quail_2_3__Unit_of_product = item;
                                                    }
                                                    if (row["Quail#3#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Quail_3_2__Product_preparation_Unit_of_product = item;
                                                    }
                                                    if (row["Quail#4#3##Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Quail_4_3__Unit_of_product = item;
                                                    }
                                                }

                                                questionnaire.Quail_1_1__Product_preparation__dilution__Product_amount = row["Quail#1#1##Product#preparation##dilution#_Product#amount"].ToString();
                                                questionnaire.Quail_1_2__Product_preparation_Unit_of_product = Quail_1_2__Product_preparation_Unit_of_product;
                                                questionnaire.Quail_1_3__Product_preparation_To_be_added_to__min_ = row["Quail#1#3##Product#preparation_To#be#added#to##min#"].ToString();
                                                questionnaire.Quail_1_4__Product_preparation_To_be_added_to__max_ = row["Quail#1#4##Product#preparation_To#be#added#to##max#"].ToString();
                                                questionnaire.Quail_1_5__Product_preparation_Unit_of_water_feed = row["Quail#1#5##Product#preparation_Unit#of#water#feed"].ToString();
                                                questionnaire.Quail_1_6__Duration_of_usage = row["Quail#1#6##Duration#of#usage"].ToString();


                                                questionnaire.Quail_2_1__Product_min = row["Quail#2#1##Product#min"].ToString();
                                                questionnaire.Quail_2_2__Product_max = row["Quail#2#2##Product#max"].ToString();
                                                questionnaire.Quail_2_3__Unit_of_product = Quail_2_3__Unit_of_product;
                                                questionnaire.Quail_2_4__Per_No__Kg_bodyweight_min = row["Quail#2#4##Per#No##Kg#bodyweight_min"].ToString();
                                                questionnaire.Quail_2_5__Per_No__Kg_bodyweight_max = row["Quail#2#5##Per#No##Kg#bodyweight_max"].ToString();
                                                questionnaire.Quail_2_6__Duration_of_usage = row["Quail#2#6##Duration#of#usage"].ToString();


                                                questionnaire.Quail_3_1__Product_preparation__dilution__Product_amount = row["Quail#3#1##Product#preparation##dilution#_Product#amount"].ToString();
                                                questionnaire.Quail_3_2__Product_preparation_Unit_of_product = Quail_3_2__Product_preparation_Unit_of_product;
                                                questionnaire.Quail_3_3__Product_preparation_To_be_added_to__min_ = row["Quail#3#3##Product#preparation_To#be#added#to##min#"].ToString();
                                                questionnaire.Quail_3_4__Product_preparation_To_be_added_to__max_ = row["Quail#3#4##Product#preparation_To#be#added#to##max#"].ToString();
                                                questionnaire.Quail_3_5__Product_preparation_Unit_of_water_feed = row["Quail#3#5##Product#preparation_Unit#of#water#feed"].ToString();
                                                questionnaire.Quail_3_6__Duration_of_usage = row["Quail#3#6##Duration#of#usage"].ToString();



                                                questionnaire.Quail_4_1__Product_min = row["Quail#4#1##Product#min"].ToString();
                                                questionnaire.Quail_4_2__Product_max = row["Quail#4#2##Product#max"].ToString();
                                                questionnaire.Quail_4_3__Unit_of_product = Quail_4_3__Unit_of_product;
                                                questionnaire.Quail_4_4_Per_No__Kg_bodyweight_min = row["Quail#4#4#Per#No##Kg#bodyweight_min"].ToString();
                                                questionnaire.Quail_4_5__Per_No__Kg_bodyweight_max = row["Quail#4#5##Per#No##Kg#bodyweight_max"].ToString();
                                                questionnaire.Quail_4_6__Duration_of_usage = row["Quail#4#6##Duration#of#usage"].ToString();

                                                #endregion

                                                #region Q.Goose
                                                var Goose_1_2__Product_preparation_Unit_of_product = "";
                                                var Goose_2_3__Unit_of_product = "";
                                                var Goose_3_2__Product_preparation_Unit_of_product = "";
                                                var Goose_4_3__Unit_of_product = "";
                                                foreach (var item in lsunitvolumn)
                                                {
                                                    if (row["Goose#1#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Goose_1_2__Product_preparation_Unit_of_product = item;
                                                    }
                                                    if (row["Goose#2#3##Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Goose_2_3__Unit_of_product = item;
                                                    }
                                                    if (row["Goose#3#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Goose_3_2__Product_preparation_Unit_of_product = item;
                                                    }
                                                    if (row["Goose#4#3##Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Goose_4_3__Unit_of_product = item;
                                                    }
                                                }

                                                questionnaire.Goose_1_1__Product_preparation__dilution__Product_amount = row["Goose#1#1##Product#preparation##dilution#_Product#amount"].ToString();
                                                questionnaire.Goose_1_2__Product_preparation_Unit_of_product = Goose_1_2__Product_preparation_Unit_of_product;
                                                questionnaire.Goose_1_3__Product_preparation_To_be_added_to__min_ = row["Goose#1#3##Product#preparation_To#be#added#to##min#"].ToString();
                                                questionnaire.Goose_1_4__Product_preparation_To_be_added_to__max_ = row["Goose#1#4##Product#preparation_To#be#added#to##max#"].ToString();
                                                questionnaire.Goose_1_5__Product_preparation_Unit_of_water_feed = row["Goose#1#5##Product#preparation_Unit#of#water#feed"].ToString();
                                                questionnaire.Goose_1_6__Duration_of_usage = row["Goose#1#6##Duration#of#usage"].ToString();


                                                questionnaire.Goose_2_1__Product_min = row["Goose#2#1##Product#min"].ToString();
                                                questionnaire.Goose_2_2__Product_max = row["Goose#2#2##Product#max"].ToString();
                                                questionnaire.Goose_2_3__Unit_of_product = Goose_2_3__Unit_of_product;
                                                questionnaire.Goose_2_4__Per_No__Kg_bodyweight_min = row["Goose#2#4##Per#No##Kg#bodyweight_min"].ToString();
                                                questionnaire.Goose_2_5__Per_No__Kg_bodyweight_max = row["Goose#2#5##Per#No##Kg#bodyweight_max"].ToString();
                                                questionnaire.Goose_2_6__Duration_of_usage = row["Goose#2#6##Duration#of#usage"].ToString();


                                                questionnaire.Goose_3_1__Product_preparation__dilution__Product_amount = row["Goose#3#1##Product#preparation##dilution#_Product#amount"].ToString();
                                                questionnaire.Goose_3_2__Product_preparation_Unit_of_product = Goose_3_2__Product_preparation_Unit_of_product;
                                                questionnaire.Goose_3_3__Product_preparation_To_be_added_to__min_ = row["Goose#3#3##Product#preparation_To#be#added#to##min#"].ToString();
                                                questionnaire.Goose_3_4__Product_preparation_To_be_added_to__max_ = row["Goose#3#4##Product#preparation_To#be#added#to##max#"].ToString();
                                                questionnaire.Goose_3_5__Product_preparation_Unit_of_water_feed = row["Goose#3#5##Product#preparation_Unit#of#water#feed"].ToString();
                                                questionnaire.Goose_3_6__Duration_of_usage = row["Goose#3#6##Duration#of#usage"].ToString();



                                                questionnaire.Goose_4_1__Product_min = row["Goose#4#1##Product#min"].ToString();
                                                questionnaire.Goose_4_2__Product_max = row["Goose#4#2##Product#max"].ToString();
                                                questionnaire.Goose_4_3__Unit_of_product = Goose_4_3__Unit_of_product;
                                                questionnaire.Goose_4_4_Per_No__Kg_bodyweight_min = row["Goose#4#4#Per#No##Kg#bodyweight_min"].ToString();
                                                questionnaire.Goose_4_5__Per_No__Kg_bodyweight_max = row["Goose#4#5##Per#No##Kg#bodyweight_max"].ToString();
                                                questionnaire.Goose_4_6__Duration_of_usage = row["Goose#4#6##Duration#of#usage"].ToString();

                                                #endregion

                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                    foreach (var item in questionnaires)
                    {
                        unitWork.Questionnaire.Insert(item);
                        unitWork.Commit();
                    }
                }

                if (filePath.Length > 0)
                {
                    DataSet ds = new DataSet();

                    string ConnectionString = "";
                    if (filePath.EndsWith(".xls"))
                    {
                        ConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", filePath);
                    }
                    else if (filePath.EndsWith(".xlsx"))
                    {
                        ConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", filePath);
                    }
                    using (OleDbConnection conn = new System.Data.OleDb.OleDbConnection(ConnectionString))
                    {
                        conn.Open();
                        using (DataTable dtExcelSchema = conn.GetSchema("Tables"))
                        {
                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();

                            string query2 = "SELECT * FROM [" + sheetName + "$QK1:UD12000]";
                            OleDbDataAdapter adapter2 = new OleDbDataAdapter(query2, conn);
                            //DataSet ds = new DataSet();
                            adapter2.Fill(ds, "Items");
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)//ds.Tables[0].Rows.Count
                                    {
                                        DataRow row = ds.Tables[0].Rows[i];
                                        foreach (var questionnaire in questionnaires)
                                        {
                                            if (questionnaire.keyupload == (i+1).ToString())
                                            {

                                                #region R.Dog
                                                var Dog_1_2__Product_preparation_Unit_of_product = "";
                                                var Dog_2_3__Unit_of_product = "";
                                                var Dog_3_2__Product_preparation_Unit_of_product = "";
                                                var Dog_4_3__Unit_of_product = "";
                                                foreach (var item in lsunitvolumn)
                                                {
                                                    if (row["Dog#1#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Dog_1_2__Product_preparation_Unit_of_product = item;
                                                    }
                                                    if (row["Dog#2#3##Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Dog_2_3__Unit_of_product = item;
                                                    }
                                                    if (row["Dog#3#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Dog_3_2__Product_preparation_Unit_of_product = item;
                                                    }
                                                    if (row["Dog#4#3##Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Dog_4_3__Unit_of_product = item;
                                                    }
                                                }

                                                questionnaire.Dog_1_1__Product_preparation__dilution__Product_amount = row["Dog#1#1##Product#preparation##dilution#_Product#amount"].ToString();
                                                questionnaire.Dog_1_2__Product_preparation_Unit_of_product = Dog_1_2__Product_preparation_Unit_of_product;
                                                questionnaire.Dog_1_3__Product_preparation_To_be_added_to__min_ = row["Dog#1#3##Product#preparation_To#be#added#to##min#"].ToString();
                                                questionnaire.Dog_1_4__Product_preparation_To_be_added_to__max_ = row["Dog#1#4##Product#preparation_To#be#added#to##max#"].ToString();
                                                questionnaire.Dog_1_5__Product_preparation_Unit_of_water_feed = row["Dog#1#5##Product#preparation_Unit#of#water#feed"].ToString();
                                                questionnaire.Dog_1_6__Duration_of_usage = row["Dog#1#6##Duration#of#usage"].ToString();


                                                questionnaire.Dog_2_1__Product_min = row["Dog#2#1##Product#min"].ToString();
                                                questionnaire.Dog_2_2__Product_max = row["Dog#2#2##Product#max"].ToString();
                                                questionnaire.Dog_2_3__Unit_of_product = Dog_2_3__Unit_of_product;
                                                questionnaire.Dog_2_4__Per_No__Kg_bodyweight_min = row["Dog#2#4##Per#No##Kg#bodyweight_min"].ToString();
                                                questionnaire.Dog_2_5__Per_No__Kg_bodyweight_max = row["Dog#2#5##Per#No##Kg#bodyweight_max"].ToString();
                                                questionnaire.Dog_2_6__Duration_of_usage = row["Dog#2#6##Duration#of#usage"].ToString();


                                                questionnaire.Dog_3_1__Product_preparation__dilution__Product_amount = row["Dog#3#1##Product#preparation##dilution#_Product#amount"].ToString();
                                                questionnaire.Dog_3_2__Product_preparation_Unit_of_product = Dog_3_2__Product_preparation_Unit_of_product;
                                                questionnaire.Dog_3_3__Product_preparation_To_be_added_to__min_ = row["Dog#3#3##Product#preparation_To#be#added#to##min#"].ToString();
                                                questionnaire.Dog_3_4__Product_preparation_To_be_added_to__max_ = row["Dog#3#4##Product#preparation_To#be#added#to##max#"].ToString();
                                                questionnaire.Dog_3_5__Product_preparation_Unit_of_water_feed = row["Dog#3#5##Product#preparation_Unit#of#water#feed"].ToString();
                                                questionnaire.Dog_3_6__Duration_of_usage = row["Dog#3#6##Duration#of#usage"].ToString();



                                                questionnaire.Dog_4_1__Product_min = row["Dog#4#1##Product#min"].ToString();
                                                questionnaire.Dog_4_2__Product_max = row["Dog#4#2##Product#max"].ToString();
                                                questionnaire.Dog_4_3__Unit_of_product = Dog_4_3__Unit_of_product;
                                                questionnaire.Dog_4_4_Per_No__Kg_bodyweight_min = row["Dog#4#4#Per#No##Kg#bodyweight_min"].ToString();
                                                questionnaire.Dog_4_5__Per_No__Kg_bodyweight_max = row["Dog#4#5##Per#No##Kg#bodyweight_max"].ToString();
                                                questionnaire.Dog_4_6__Duration_of_usage = row["Dog#4#6##Duration#of#usage"].ToString();

                                                #endregion

                                                #region S.Cat
                                                var Cat_1_2__Product_preparation_Unit_of_product = "";
                                                var Cat_2_3__Unit_of_product = "";
                                                var Cat_3_2__Product_preparation_Unit_of_product = "";
                                                var Cat_4_3__Unit_of_product = "";
                                                foreach (var item in lsunitvolumn)
                                                {
                                                    if (row["Cat#1#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Cat_1_2__Product_preparation_Unit_of_product = item;
                                                    }
                                                    if (row["Cat#2#3##Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Cat_2_3__Unit_of_product = item;
                                                    }
                                                    if (row["Cat#3#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Cat_3_2__Product_preparation_Unit_of_product = item;
                                                    }
                                                    if (row["Cat#4#3##Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Cat_4_3__Unit_of_product = item;
                                                    }
                                                }

                                                questionnaire.Cat_1_1__Product_preparation__dilution__Product_amount = row["Cat#1#1##Product#preparation##dilution#_Product#amount"].ToString();
                                                questionnaire.Cat_1_2__Product_preparation_Unit_of_product = Cat_1_2__Product_preparation_Unit_of_product;
                                                questionnaire.Cat_1_3__Product_preparation_To_be_added_to__min_ = row["Cat#1#3##Product#preparation_To#be#added#to##min#"].ToString();
                                                questionnaire.Cat_1_4__Product_preparation_To_be_added_to__max_ = row["Cat#1#4##Product#preparation_To#be#added#to##max#"].ToString();
                                                questionnaire.Cat_1_5__Product_preparation_Unit_of_water_feed = row["Cat#1#5##Product#preparation_Unit#of#water#feed"].ToString();
                                                questionnaire.Cat_1_6__Duration_of_usage = row["Cat#1#6##Duration#of#usage"].ToString();


                                                questionnaire.Cat_2_1__Product_min = row["Cat#2#1##Product#min"].ToString();
                                                questionnaire.Cat_2_2__Product_max = row["Cat#2#2##Product#max"].ToString();
                                                questionnaire.Cat_2_3__Unit_of_product = Cat_2_3__Unit_of_product;
                                                questionnaire.Cat_2_4__Per_No__Kg_bodyweight_min = row["Cat#2#4##Per#No##Kg#bodyweight_min"].ToString();
                                                questionnaire.Cat_2_5__Per_No__Kg_bodyweight_max = row["Cat#2#5##Per#No##Kg#bodyweight_max"].ToString();
                                                questionnaire.Cat_2_6__Duration_of_usage = row["Cat#2#6##Duration#of#usage"].ToString();


                                                questionnaire.Cat_3_1__Product_preparation__dilution__Product_amount = row["Cat#3#1##Product#preparation##dilution#_Product#amount"].ToString();
                                                questionnaire.Cat_3_2__Product_preparation_Unit_of_product = Cat_3_2__Product_preparation_Unit_of_product;
                                                questionnaire.Cat_3_3__Product_preparation_To_be_added_to__min_ = row["Cat#3#3##Product#preparation_To#be#added#to##min#"].ToString();
                                                questionnaire.Cat_3_4__Product_preparation_To_be_added_to__max_ = row["Cat#3#4##Product#preparation_To#be#added#to##max#"].ToString();
                                                questionnaire.Cat_3_5__Product_preparation_Unit_of_water_feed = row["Cat#3#5##Product#preparation_Unit#of#water#feed"].ToString();
                                                questionnaire.Cat_3_6__Duration_of_usage = row["Cat#3#6##Duration#of#usage"].ToString();



                                                questionnaire.Cat_4_1__Product_min = row["Cat#4#1##Product#min"].ToString();
                                                questionnaire.Cat_4_2__Product_max = row["Cat#4#2##Product#max"].ToString();
                                                questionnaire.Cat_4_3__Unit_of_product = Cat_4_3__Unit_of_product;
                                                questionnaire.Cat_4_4_Per_No__Kg_bodyweight_min = row["Cat#4#4#Per#No##Kg#bodyweight_min"].ToString();
                                                questionnaire.Cat_4_5__Per_No__Kg_bodyweight_max = row["Cat#4#5##Per#No##Kg#bodyweight_max"].ToString();
                                                questionnaire.Cat_4_6__Duration_of_usage = row["Cat#4#6##Duration#of#usage"].ToString();

                                                #endregion

                                                #region T.Calf
                                                var Calf_1_2__Product_preparation_Unit_of_product = "";
                                                var Calf_2_3__Unit_of_product = "";
                                                var Calf_3_2__Product_preparation_Unit_of_product = "";
                                                var Calf_4_3__Unit_of_product = "";
                                                foreach (var item in lsunitvolumn)
                                                {
                                                    if (row["Calf#1#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Calf_1_2__Product_preparation_Unit_of_product = item;
                                                    }
                                                    if (row["Calf#2#3##Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Calf_2_3__Unit_of_product = item;
                                                    }
                                                    if (row["Calf#3#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Calf_3_2__Product_preparation_Unit_of_product = item;
                                                    }
                                                    if (row["Calf#4#3##Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Calf_4_3__Unit_of_product = item;
                                                    }
                                                }

                                                questionnaire.Calf_1_1__Product_preparation__dilution__Product_amount = row["Calf#1#1##Product#preparation##dilution#_Product#amount"].ToString();
                                                questionnaire.Calf_1_2__Product_preparation_Unit_of_product = Calf_1_2__Product_preparation_Unit_of_product;
                                                questionnaire.Calf_1_3__Product_preparation_To_be_added_to__min_ = row["Calf#1#3##Product#preparation_To#be#added#to##min#"].ToString();
                                                questionnaire.Calf_1_4__Product_preparation_To_be_added_to__max_ = row["Calf#1#4##Product#preparation_To#be#added#to##max#"].ToString();
                                                questionnaire.Calf_1_5__Product_preparation_Unit_of_water_feed = row["Calf#1#5##Product#preparation_Unit#of#water#feed"].ToString();
                                                questionnaire.Calf_1_6__Duration_of_usage = row["Calf#1#6##Duration#of#usage"].ToString();


                                                questionnaire.Calf_2_1__Product_min = row["Calf#2#1##Product#min"].ToString();
                                                questionnaire.Calf_2_2__Product_max = row["Calf#2#2##Product#max"].ToString();
                                                questionnaire.Calf_2_3__Unit_of_product = Calf_2_3__Unit_of_product;
                                                questionnaire.Calf_2_4__Per_No__Kg_bodyweight_min = row["Calf#2#4##Per#No##Kg#bodyweight_min"].ToString();
                                                questionnaire.Calf_2_5__Per_No__Kg_bodyweight_max = row["Calf#2#5##Per#No##Kg#bodyweight_max"].ToString();
                                                questionnaire.Calf_2_6__Duration_of_usage = row["Calf#2#6##Duration#of#usage"].ToString();


                                                questionnaire.Calf_3_1__Product_preparation__dilution__Product_amount = row["Calf#3#1##Product#preparation##dilution#_Product#amount"].ToString();
                                                questionnaire.Calf_3_2__Product_preparation_Unit_of_product = Calf_3_2__Product_preparation_Unit_of_product;
                                                questionnaire.Calf_3_3__Product_preparation_To_be_added_to__min_ = row["Calf#3#3##Product#preparation_To#be#added#to##min#"].ToString();
                                                questionnaire.Calf_3_4__Product_preparation_To_be_added_to__max_ = row["Calf#3#4##Product#preparation_To#be#added#to##max#"].ToString();
                                                questionnaire.Calf_3_5__Product_preparation_Unit_of_water_feed = row["Calf#3#5##Product#preparation_Unit#of#water#feed"].ToString();
                                                questionnaire.Calf_3_6__Duration_of_usage = row["Calf#3#6##Duration#of#usage"].ToString();



                                                questionnaire.Calf_4_1__Product_min = row["Calf#4#1##Product#min"].ToString();
                                                questionnaire.Calf_4_2__Product_max = row["Calf#4#2##Product#max"].ToString();
                                                questionnaire.Calf_4_3__Unit_of_product = Calf_4_3__Unit_of_product;
                                                questionnaire.Calf_4_4_Per_No__Kg_bodyweight_min = row["Calf#4#4#Per#No##Kg#bodyweight_min"].ToString();
                                                questionnaire.Calf_4_5__Per_No__Kg_bodyweight_max = row["Calf#4#5##Per#No##Kg#bodyweight_max"].ToString();
                                                questionnaire.Calf_4_6__Duration_of_usage = row["Calf#4#6##Duration#of#usage"].ToString();

                                                #endregion

                                                #region R.Chick
                                                var Chick_1_2__Product_preparation_Unit_of_product = "";
                                                var Chick_2_3__Unit_of_product = "";
                                                var Chick_3_2__Product_preparation_Unit_of_product = "";
                                                var Chick_4_3__Unit_of_product = "";
                                                foreach (var item in lsunitvolumn)
                                                {

                                                    if (row["Chick#Duckling#1#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Chick_1_2__Product_preparation_Unit_of_product = item;
                                                    }
                                                    if (row["Chick#Duckling#2#3##Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Chick_2_3__Unit_of_product = item;
                                                    }
                                                    if (row["Chick#Duckling#3#2##Product#preparation_Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Chick_3_2__Product_preparation_Unit_of_product = item;
                                                    }
                                                    if (row["Chick#Duckling#4#3##Unit#of#product"].ToString().Trim() == item)
                                                    {
                                                        Chick_4_3__Unit_of_product = item;
                                                    }
                                                }
                                                questionnaire.Chick_1_1__Product_preparation__dilution__Product_amount = row["Chick#Duckling#1#1##Product#preparation##dilution#_Product#amoun"].ToString();
                                                questionnaire.Chick_1_2__Product_preparation_Unit_of_product = Chick_1_2__Product_preparation_Unit_of_product;
                                                questionnaire.Chick_1_3__Product_preparation_To_be_added_to__min_ = row["Chick#Duckling#1#3##Product#preparation_To#be#added#to##min#"].ToString();
                                                questionnaire.Chick_1_4__Product_preparation_To_be_added_to__max_ = row["Chick#Duckling#1#4##Product#preparation_To#be#added#to##max#"].ToString();
                                                questionnaire.Chick_1_5__Product_preparation_Unit_of_water_feed = row["Chick#Duckling#1#5##Product#preparation_Unit#of#water#feed"].ToString();
                                                questionnaire.Chick_1_6__Duration_of_usage = row["Chick#Duckling#1#6##Duration#of#usage"].ToString();


                                                questionnaire.Chick_2_1__Product_min = row["Chick#Duckling#2#1##Product#min"].ToString();
                                                questionnaire.Chick_2_2__Product_max = row["Chick#Duckling#2#2##Product#max"].ToString();
                                                questionnaire.Chick_2_3__Unit_of_product = Chick_2_3__Unit_of_product;
                                                questionnaire.Chick_2_4__Per_No__Kg_bodyweight_min = row["Chick#Duckling#2#4##Per#No##Kg#bodyweight_min"].ToString();
                                                questionnaire.Chick_2_5__Per_No__Kg_bodyweight_max = row["Chick#Duckling#2#5##Per#No##Kg#bodyweight_max"].ToString();
                                                questionnaire.Chick_2_6__Duration_of_usage = row["Chick#Duckling#2#6##Duration#of#usage"].ToString();


                                                questionnaire.Chick_3_1__Product_preparation__dilution__Product_amount = row["Chick#Duckling#3#1##Product#preparation##dilution#_Product#amoun"].ToString();
                                                questionnaire.Chick_3_2__Product_preparation_Unit_of_product = Chick_3_2__Product_preparation_Unit_of_product;
                                                questionnaire.Chick_3_3__Product_preparation_To_be_added_to__min_ = row["Chick#Duckling#3#3##Product#preparation_To#be#added#to##min#"].ToString();
                                                questionnaire.Chick_3_4__Product_preparation_To_be_added_to__max_ = row["Chick#Duckling#3#4##Product#preparation_To#be#added#to##max#"].ToString();
                                                questionnaire.Chick_3_5__Product_preparation_Unit_of_water_feed = row["Chick#Duckling#3#5##Product#preparation_Unit#of#water#feed"].ToString();
                                                questionnaire.Chick_3_6__Duration_of_usage = row["Chick#Duckling#3#6##Duration#of#usage"].ToString();



                                                questionnaire.Chick_4_1__Product_min = row["Chick#Duckling#4#1##Product#min"].ToString();
                                                questionnaire.Chick_4_2__Product_max = row["Chick#Duckling#4#2##Product#max"].ToString();
                                                questionnaire.Chick_4_3__Unit_of_product = Chick_4_3__Unit_of_product;
                                                questionnaire.Chick_4_4_Per_No__Kg_bodyweight_min = row["Chick#Duckling#4#4#Per#No##Kg#bodyweight_min"].ToString();
                                                questionnaire.Chick_4_5__Per_No__Kg_bodyweight_max = row["Chick#Duckling#4#5##Per#No##Kg#bodyweight_max"].ToString();
                                                questionnaire.Chick_4_6__Duration_of_usage = row["Chick#Duckling#4#6##Duration#of#usage"].ToString();

                                                #endregion
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                    
                }

                foreach (var item in questionnaires)
                {
                    unitWork.Questionnaire.Update(item);
                    unitWork.Commit();
                }
            }
            catch (Exception ex) { throw ex; return ex.Message; }
            return "";
        }


		public string UpdateQuestionnaire(string filePath, User user,UnitWork unitWork)
		{
			return ImportQuestionnaire(filePath, user, unitWork);
			StringBuilder strValidations = new StringBuilder(string.Empty);
			try
			{
				//EhrDbContext context = new EhrDbContext();
				List<Questionnaire> questionnaires = new List<Questionnaire>();
				string[] lsunitvolumn = { "lít", "kg", "mg", "g", "UI","IU", "ml" };
				if (filePath.Length > 0)
				{
					DataSet ds = new DataSet();

					string ConnectionString = "";
					if (filePath.EndsWith(".xls"))
					{
						ConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", filePath);
					}
					else if (filePath.EndsWith(".xlsx"))
					{
						ConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", filePath);
					}
					using (OleDbConnection conn = new System.Data.OleDb.OleDbConnection(ConnectionString))
					{
						conn.Open();
						using (DataTable dtExcelSchema = conn.GetSchema("Tables"))
						{
							string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
							string query = "SELECT * FROM [" + sheetName + "$A1:IB12000]";
							OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
							//DataSet ds = new DataSet();
							adapter.Fill(ds, "Items");
							if (ds.Tables.Count > 0)
							{
								if (ds.Tables[0].Rows.Count > 0)
								{
									for (int i = 0; i < ds.Tables[0].Rows.Count; i++)//ds.Tables[0].Rows.Count
									{
										DataRow row = ds.Tables[0].Rows[i];
										string product_code = row["A2##Product#code"].ToString();
										Questionnaire questionnaire = unitWork.Questionnaire.Get(c=>c.A3__Product_name.Equals(product_code)).FirstOrDefault();
										if (questionnaire == null) questionnaire = new Questionnaire();
										
										questionnaire.CreateBy = user;
										questionnaire.keyupload = row[0].ToString().Trim();
										if (DateTime.TryParse(row[1].ToString().Trim(), out DateTime sdate))
										{
											questionnaire.D_u_th_i_gian = sdate;
										}
										#region A.General information
										var A1__Product_origin = "";
										if (row["A1##Product#origin"].ToString().Trim() == "Imported products")
										{
											A1__Product_origin = "Sản phẩm nhập khẩu";
										}
										else if (row["A1##Product#origin"].ToString().Trim() == "Domestic products")
										{
											A1__Product_origin = "Sản phẩm sản xuất trong nước";
										}
										var A5__Type_of_product = "";
										if (row["A5##Type#of#product"].ToString().Trim() == "Powder (dạng bột)")
										{
											A5__Type_of_product = "Dạng bột";
										}
										else if (row["A5##Type#of#product"].ToString().Trim() == "Liquid (dạng dung dịch)")
										{
											A5__Type_of_product = "Dạng dung dịch";
										}
										var A6__Other_subtance_in_product = "";
										if (row["A6##Other#subtance#in#product"].ToString().Trim() == "No")
										{
											A6__Other_subtance_in_product = "Không";
										}
										else if (row["A6##Other#subtance#in#product"].ToString().Trim() == "Yes")
										{
											A6__Other_subtance_in_product = "Có";
										}
										var A8__Unit_of_volume_of_product = "";
										string temp_unit = row["A8##Unit#of#volume#of#product"].ToString().Trim();
										if (temp_unit == "UI") temp_unit = "IU";
										foreach (var item in lsunitvolumn)
										{
											if (temp_unit == item)
											{
												A8__Unit_of_volume_of_product = item;
											}
										}
										questionnaire.A1__Product_origin = A1__Product_origin;
										questionnaire.A2__Product_code = row["A2##Product#code"].ToString();
										questionnaire.A3__Product_name = row["A3##Product#name"].ToString();
										questionnaire.A4__Company = row["A4##Company"].ToString();
										questionnaire.A5__Type_of_product = A5__Type_of_product;
										questionnaire.A6__Other_subtance_in_product = A6__Other_subtance_in_product;
										questionnaire.A7__Volume_of_product = row["A7##Volume#of#product"].ToString();
										questionnaire.A8__Unit_of_volume_of_product = A8__Unit_of_volume_of_product;
										questionnaire.A9__Other_volume_of_product = row["A9##Other#volume#of#product"].ToString();
										#endregion

										#region B.Information related to antimicrobial
										var B2_3__Units_of_antimicrobial_1 = "";
										var B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_ = "";
										var B2_7__Units_of_product__link_to_question_B2_4_ = "";
										var B3_3__Units_of_antimicrobial_2 = "";
										var B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_ = "";
										var B3_7__Units_of_product__link_to_question_B3_4_ = "";
										var B4_3__Units_of_antimicrobial_3 = "";
										var B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_ = "";
										var B4_7__Units_of_product__link_to_question_B4_4_ = "";
										var B5_3__Units_of_antimicrobial_4 = "";
										var B5_5__Units_of_antimicrobial_4__link_to_question_5_4_ = "";
										var B5_7__Units_of_product__link_to_question_B5_4_ = "";
										foreach (var item in lsunitvolumn)
										{
											temp_unit = row["B2#3##Units#of#antimicrobial#1"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";

											if (temp_unit == item)
											{
												B2_3__Units_of_antimicrobial_1 = item;
											}

											temp_unit = row["B2#5##Units#of#antimicrobial#1##link#to#question#B2#4#"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";
											if (temp_unit == item)
											{
												B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_ = item;
											}
											temp_unit = row["B2#7##Units#of#product##link#to#question#B2#4#"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";

											if (temp_unit == item)
											{
												B2_7__Units_of_product__link_to_question_B2_4_ = item;
											}

											temp_unit = row["B3#3##Units#of#antimicrobial#2"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";

											if (temp_unit == item)
											{
												B3_3__Units_of_antimicrobial_2 = item;
											}

											temp_unit = row["B3#5##Units#of#antimicrobial#2##link#to#question#B3#4#"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";

											if (temp_unit == item)											
											{
												B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_ = item;
											}

											temp_unit = row["B3#7##Units#of#product##link#to#question#B3#4#"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";
											if (temp_unit == item)
											{
												B3_7__Units_of_product__link_to_question_B3_4_ = item;
											}

											temp_unit = row["B4#3##Units#of#antimicrobial#3"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";
											if (temp_unit == item)
											{
												B4_3__Units_of_antimicrobial_3 = item;
											}

											temp_unit = row["B4#5##Units#of#antimicrobial#3##link#to#question#B4#4#"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";
											if (temp_unit == item)
											{
												B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_ = item;
											}

											temp_unit = row["B4#7##Units#of#product##link#to#question#B4#4#"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";
											if (temp_unit == item)
											{
												B4_7__Units_of_product__link_to_question_B4_4_ = item;
											}
											temp_unit = row["B5#3##Units#of#antimicrobial#4"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";
											if (temp_unit == item)
											{
												B5_3__Units_of_antimicrobial_4 = item;
											}
											temp_unit = row["B5#5##Units#of#antimicrobial#4##link#to#question#5#4#"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";
											if (temp_unit == item)											
											{
												B5_5__Units_of_antimicrobial_4__link_to_question_5_4_ = item;
											}
											temp_unit = row["B5#7##Units#of#product##link#to#question#B5#4#"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";
											if (temp_unit == item)
											{
												B5_7__Units_of_product__link_to_question_B5_4_ = item;
											}

										}

										if (row["B1##Number#of#antimicrobials#in#product"].ToString().Trim() != "")
										{
											if (row["B1##Number#of#antimicrobials#in#product"].ToString().Trim() == "NA")
											{
												questionnaire.B1__Number_of_antimicrobials_in_product = -1;
											}
											else
											{
												questionnaire.B1__Number_of_antimicrobials_in_product = int.Parse(row["B1##Number#of#antimicrobials#in#product"].ToString());
											}
										}
										#region Antimicrobials 1 
										if (row["B2#1##Antimicrobial#1"].ToString().ToUpper().Trim() == "NA" || row["B2#1##Antimicrobial#1"].ToString().ToUpper().Trim() == "")
										{
											questionnaire.B2_1__Antimicrobial_1 = "NONE";
										}
										else if (row["B2#1##Antimicrobial#1"].ToString().ToUpper().Trim() == "L-CARNITINE")
										{
											questionnaire.B2_1__Antimicrobial_1 = "L_CARNITINE";
										}
										else
										{
											questionnaire.B2_1__Antimicrobial_1 = row["B2#1##Antimicrobial#1"].ToString().ToUpper().Trim();
										}
										if (row["B2#2##Strength#of#antimicrobial#1"].ToString() != "")
										{
											if (row["B2#2##Strength#of#antimicrobial#1"].ToString() == "NA")
											{
												questionnaire.B2_2__Strength_of_antimicrobial_1 = -1;
											}
											else
											{
												questionnaire.B2_2__Strength_of_antimicrobial_1 = double.Parse(row["B2#2##Strength#of#antimicrobial#1"].ToString());
											}
										}
										questionnaire.B2_3__Units_of_antimicrobial_1 = B2_3__Units_of_antimicrobial_1;
										questionnaire.B2_4__Per_amount_of_product__antimicrobial_1_ = row["B2#4##Per#amount#of#product##antimicrobial#1#"].ToString();
										questionnaire.B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_ = B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_;
										questionnaire.B2_6__Per_amount_of_product__volume_of_product___link_to_question_B2_4_ = row["B2#6##Per#amount#of#product##volume#of#product###link#to#questio"].ToString();
										questionnaire.B2_7__Units_of_product__link_to_question_B2_4_ = B2_7__Units_of_product__link_to_question_B2_4_;

										#endregion
										#region Antimicrobials 2
										if (row["B3#1##Antimicrobial#2"].ToString().ToUpper().Trim() == "NA" || row["B3#1##Antimicrobial#2"].ToString().ToUpper().Trim() == "")
										{
											questionnaire.B3_1__Antimicrobial_2 = "NONE";
										}
										else if (row["B3#1##Antimicrobial#2"].ToString().ToUpper().Trim() == "L-CARNITINE")
										{
											questionnaire.B3_1__Antimicrobial_2 = "L_CARNITINE";
										}
										else
										{
											questionnaire.B3_1__Antimicrobial_2 = row["B3#1##Antimicrobial#2"].ToString().ToUpper().Trim();
										}
										if (row["B3#2##Strength#of#antimicrobial#2"].ToString() != "")
										{
											if (row["B3#2##Strength#of#antimicrobial#2"].ToString() == "NA")
											{
												questionnaire.B3_2__Strength_of_antimicrobial_2 = -1;
											}
											else
											{
												questionnaire.B3_2__Strength_of_antimicrobial_2 = double.Parse(row["B3#2##Strength#of#antimicrobial#2"].ToString());
											}
										}
										questionnaire.B3_3__Units_of_antimicrobial_2 = B3_3__Units_of_antimicrobial_2;
										questionnaire.B3_4__Per_amount_of_product__antimicrobial_2_ = row["B3#4##Per#amount#of#product##antimicrobial#2#"].ToString();
										questionnaire.B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_ = B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_;
										questionnaire.B3_6__Per_amount_of_product__volume_of_product___link_to_question_B3_4_ = row["B3#6##Per#amount#of#product##volume#of#product###link#to#questio"].ToString();
										questionnaire.B3_7__Units_of_product__link_to_question_B3_4_ = B3_7__Units_of_product__link_to_question_B3_4_;
										#endregion
										#region Antimicrobials 3
										if (row["B4#1##Antimicrobial#3"].ToString().ToUpper().Trim() == "NA" || row["B4#1##Antimicrobial#3"].ToString().ToUpper().Trim() == "")
										{
											questionnaire.B4_1__Antimicrobial_3 = "NONE";
										}
										else if (row["B4#1##Antimicrobial#3"].ToString().ToUpper().Trim() == "L-CARNITINE")
										{
											questionnaire.B4_1__Antimicrobial_3 = "L_CARNITINE";
										}
										else
										{
											questionnaire.B4_1__Antimicrobial_3 = row["B4#1##Antimicrobial#3"].ToString().ToUpper().Trim();
										}
										var a = row["B3#2##Strength#of#antimicrobial#2"].ToString();
										if (row["B4#2##Strength#of#antimicrobial#3"].ToString() != "")
										{
											if (row["B4#2##Strength#of#antimicrobial#3"].ToString() == "NA")
											{
												questionnaire.B4_2__Strength_of_antimicrobial_3 = -1;
											}
											else
											{
												questionnaire.B4_2__Strength_of_antimicrobial_3 = double.Parse(row["B4#2##Strength#of#antimicrobial#3"].ToString());
											}
										}
										questionnaire.B4_3__Units_of_antimicrobial_3 = B4_3__Units_of_antimicrobial_3;
										questionnaire.B4_4__Per_amount_of_product__antimicrobial_3_ = row["B4#4##Per#amount#of#product##antimicrobial#3#"].ToString();
										questionnaire.B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_ = B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_;
										questionnaire.B4_6__Per_amount_of_product__volume_of_product___link_to_question_B4_4_ = row["B4#6##Per#amount#of#product##volume#of#product###link#to#questio"].ToString();
										questionnaire.B4_7__Units_of_product__link_to_question_B4_4_ = B4_7__Units_of_product__link_to_question_B4_4_;
										#endregion
										#region Antimicrobials 4
										if (row["B5#1##Antimicrobial#4"].ToString().ToUpper().Trim() == "NA" || row["B5#1##Antimicrobial#4"].ToString().ToUpper().Trim() == "")
										{
											questionnaire.B5_1__Antimicrobial_4 = "NONE";
										}
										else if (row["B5#1##Antimicrobial#4"].ToString().ToUpper().Trim() == "L-CARNITINE")
										{
											questionnaire.B5_1__Antimicrobial_4 = "L_CARNITINE";
										}
										else
										{
											questionnaire.B5_1__Antimicrobial_4 = row["B5#1##Antimicrobial#4"].ToString().ToUpper().Trim();
										}
										if (row["B5#2##Strength#of#antimicrobial#4"].ToString() != "" && row["B5#2##Strength#of#antimicrobial#4"].ToString() != "NA")
										{
											if (row["B5#2##Strength#of#antimicrobial#4"].ToString().Trim() == "NA")
											{
												questionnaire.B5_2__Strength_of_antimicrobial_4 = -1;
											}
											else
											{
												questionnaire.B5_2__Strength_of_antimicrobial_4 = double.Parse(row["B5#2##Strength#of#antimicrobial#4"].ToString());
											}
										}
										questionnaire.B5_3__Units_of_antimicrobial_4 = B5_3__Units_of_antimicrobial_4;
										questionnaire.B5_4__Per_amount_of_product__antimicrobial_4_ = row["B5#4##Per#amount#of#product##antimicrobial#4#"].ToString();
										questionnaire.B5_5__Units_of_antimicrobial_4__link_to_question_5_4_ = B5_5__Units_of_antimicrobial_4__link_to_question_5_4_;
										questionnaire.B5_6__Per_amount_of_product__volume_of_product___link_to_question_B5_4_ = row["B5#6##Per#amount#of#product##volume#of#product###link#to#questio"].ToString();
										questionnaire.B5_7__Units_of_product__link_to_question_B5_4_ = B5_7__Units_of_product__link_to_question_B5_4_;
										#endregion
										string[] lsPetDefault = { "BUFFALO", "CATTLE", "POULTRY", "PIG", "DOG", "CAT", "GOAT", "QUAIL", "SHEEP", "MUSCOVY_DUCK", "DUCK", "GOOSE", "HORSE", "CHICKEN", "PIGLET", "CALF", "CHICK" };
										string[] lsRouteDefault = { "ORAL", "INJECTABLE", "WATER", "FEED" };
										var lsPetX = row["B6##Target#species"].ToString().Split(',').ToArray();
										List<string> lsPet = new List<string>();
										foreach (var item in lsPetX)
										{
											lsPet.Add(item.Trim());
										}
										var lsroute = row["B7##Administration#route"].ToString().Split(',').ToArray();
										var lsPetSelect = new List<Pet>(); var lsRouteSelect = new List<Route>();
										foreach (var item in lsPet)
										{
											var s = item.Trim().ToUpper();
											if (item != "" && lsPetDefault.Contains(s))
											{
												Pet pet = (Pet)Enum.Parse(typeof(Pet), s, true);
												if (System.Enum.IsDefined(typeof(Pet), pet))
												{
													lsPetSelect.Add(pet);
												}
											}
										}
										foreach (var item in lsroute)
										{
											var s = item.Trim().ToUpper();
											if (item != "" && lsRouteDefault.Contains(s))
											{
												Route route = (Route)Enum.Parse(typeof(Route), s, true);
												if (System.Enum.IsDefined(typeof(Route), route))
												{
													lsRouteSelect.Add(route);
												}
											}
										}
										var lspetsave = "";
										var lsroutesave = "";
										if (lsPetSelect != null)
										{
											var count = 0;
											foreach (var item in lsPetSelect)
											{
												count++;
												if (count == lsPetSelect.Count)
												{
													lspetsave += item;
												}
												else
												{
													lspetsave += item + ",";
												}
											}
										}
										if (lsRouteSelect != null)
										{
											var count = 0;
											foreach (var item in lsRouteSelect)
											{
												count++;
												if (count == lsRouteSelect.Count)
												{
													lsroutesave += item;
												}
												else
												{
													lsroutesave += item + ",";
												}
											}
										}
										questionnaire.B6__Target_species_x = lspetsave;
										questionnaire.B7__Administration_route = lsroutesave;
										#endregion

										#region C_ Heo
										var C1_2__Product_preparation_Unit_of_product = "";
										var C2_3__Unit_of_product = "";
										var C3_2__Product_preparation_Unit_of_product = "";
										var C4_3__Unit_of_product = "";
										foreach (var item in lsunitvolumn)
										{
											temp_unit = row["C1#2##Product#preparation_Unit#of#product"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";
											if (temp_unit == item)
											{
												C1_2__Product_preparation_Unit_of_product = item;
											}
											temp_unit = row["C2#3##Unit#of#product"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";
											if (temp_unit == item)										
											{
												C2_3__Unit_of_product = item;
											}
											temp_unit = row["C3#2##Product#preparation_Unit#of#product"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";
											if (temp_unit == item)											
											{
												C3_2__Product_preparation_Unit_of_product = item;
											}
											temp_unit = row["C4#3##Unit#of#product"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";
											if (temp_unit == item)											
											{
												C4_3__Unit_of_product = item;
											}
										}
										#region C_1_ Product preparation (dilution) _pig_prevention purpose
										questionnaire.C1_1__Product_preparation__dilution__Product_amount = row["C1#1##Product#preparation##dilution#_Product#amount"].ToString();
										questionnaire.C1_2__Product_preparation_Unit_of_product = C1_2__Product_preparation_Unit_of_product;
										questionnaire.C1_3__Product_preparation_To_be_added_to__min_ = row["C1#3##Product#preparation_To#be#added#to##min#"].ToString();
										questionnaire.C1_4__Product_preparation_To_be_added_to__max_ = row["C1#4##Product#preparation_To#be#added#to##max#"].ToString();
										questionnaire.C1_5__Product_preparation_Unit_of_water_feed = row["C1#5##Product#preparation_Unit#of#water#feed"].ToString();
										questionnaire.C1_6__Duration_of_usage__min__max_ = row["C1#6##Duration#of#usage##min##max#"].ToString();
										#endregion

										#region C.2 Guidelines referred to bodyweight_pig_prevention purpose
										questionnaire.C2_1__Product_min = row["C2#1##Product#min"].ToString();
										questionnaire.C2_2__Product_max = row["C2#2##Product#max"].ToString();
										questionnaire.C2_3__Unit_of_product = C2_3__Unit_of_product;
										questionnaire.C2_4__Per_No__Kg_bodyweight_min = row["C2#4##Per#No##Kg#bodyweight_min"].ToString();
										questionnaire.C2_5__Per_No__Kg_bodyweight_max = row["C2#5##Per#No##Kg#bodyweight_max"].ToString();
										questionnaire.C2_6__Duration_of_usage = row["C2#6##Duration#of#usage"].ToString();
										#endregion

										#region C_3 Product preparation (dilution) _pig_treatment purpose
										questionnaire.C3_1__Product_preparation__dilution__Product_amount = row["C3#1##Product#preparation##dilution#_Product#amount"].ToString();
										questionnaire.C3_2__Product_preparation_Unit_of_product = C3_2__Product_preparation_Unit_of_product;
										questionnaire.C3_3__Product_preparation_To_be_added_to__min_ = row["C3#3##Product#preparation_To#be#added#to##min#"].ToString();
										questionnaire.C3_4__Product_preparation_To_be_added_to__max_ = row["C3#4##Product#preparation_To#be#added#to##max#"].ToString();
										questionnaire.C3_5__Product_preparation_Unit_of_water_feed = row["C3#5##Product#preparation_Unit#of#water#feed"].ToString();
										questionnaire.C3_6__Duration_of_usage = row["C3#6##Duration#of#usage"].ToString();
										#endregion

										#region C_4 Guidelines referred to bodyweight_pig_treatment purpose                  
										questionnaire.C4_1__Product_min = row["C4#1##Product#min"].ToString();
										questionnaire.C4_2__Product_max = row["C4#2##Product#max"].ToString();
										questionnaire.C4_3__Unit_of_product = C4_3__Unit_of_product;
										questionnaire.C4_4__Per_No__Kg_bodyweight_min = row["C4#4##Per#No##Kg#bodyweight_min"].ToString();
										questionnaire.C4_5__Per_No__Kg_bodyweight_max = row["C4#5##Per#No##Kg#bodyweight_max"].ToString();
										questionnaire.C4_6__Duration_of_usage = row["C4#6##Duration#of#usage"].ToString();
										#endregion

										#endregion

										#region D Động vật nhai lại
										var D1_2__Product_preparation_Unit_of_product = "";
										var D2_3__Unit_of_product = "";
										var D3_2__Product_preparation_Unit_of_product = "";
										var D4_3__Unit_of_product = "";
										foreach (var item in lsunitvolumn)
										{
											temp_unit = row["D1#2##Product#preparation_Unit#of#product"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";
											if (temp_unit == item)											
											{
												D1_2__Product_preparation_Unit_of_product = item;
											}

											temp_unit = row["D2#3##Unit#of#product"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";
											if (temp_unit == item)
											{
												D2_3__Unit_of_product = item;
											}

											temp_unit = row["D3#2##Product#preparation_Unit#of#product"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";
											if (temp_unit == item)											
											{
												D3_2__Product_preparation_Unit_of_product = item;
											}

											temp_unit = row["D4#3##Unit#of#product"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";
											if (temp_unit == item)											
											{
												D4_3__Unit_of_product = item;
											}
										}
										#region C_1_ Product preparation (dilution) _pig_prevention purpose
										questionnaire.D1_1__Product_preparation__dilution__Product_amount = row["D1#1##Product#preparation##dilution#_Product#amount"].ToString();
										questionnaire.D1_2__Product_preparation_Unit_of_product = D1_2__Product_preparation_Unit_of_product;
										questionnaire.D1_3__Product_preparation_To_be_added_to__min_ = row["D1#3##Product#preparation_To#be#added#to##min#"].ToString();
										questionnaire.D1_4__Product_preparation_To_be_added_to__max_ = row["D1#4##Product#preparation_To#be#added#to##max#"].ToString();
										questionnaire.D1_5__Product_preparation_Unit_of_water_feed = row["D1#5##Product#preparation_Unit#of#water#feed"].ToString();
										questionnaire.D1_6__Duration_of_usage = row["D1#6##Duration#of#usage"].ToString();
										#endregion

										#region C.2 Guidelines referred to bodyweight_pig_prevention purpose
										questionnaire.D2_1__Product_min = row["D2#1##Product#min"].ToString();
										questionnaire.D2_2__Product_max = row["D2#2##Product#max"].ToString();
										questionnaire.D2_3__Unit_of_product = D2_3__Unit_of_product;
										questionnaire.D2_4__Per_No__Kg_bodyweight_min = row["D2#4##Per#No##Kg#bodyweight_min"].ToString();
										questionnaire.D2_5__Per_No__Kg_bodyweight_max = row["D2#5##Per#No##Kg#bodyweight_max"].ToString();
										questionnaire.D2_6__Duration_of_usage = row["D2#6##Duration#of#usage"].ToString();
										#endregion

										#region C_3 Product preparation (dilution) _pig_treatment purpose
										questionnaire.D3_1__Product_preparation__dilution__Product_amount = row["D3#1##Product#preparation##dilution#_Product#amount"].ToString();
										questionnaire.D3_2__Product_preparation_Unit_of_product = D3_2__Product_preparation_Unit_of_product;
										questionnaire.D3_3__Product_preparation_To_be_added_to__min_ = row["D3#3##Product#preparation_To#be#added#to##min#"].ToString();
										questionnaire.D3_4__Product_preparation_To_be_added_to__max_ = row["D3#4##Product#preparation_To#be#added#to##max#"].ToString();
										questionnaire.D3_5__Product_preparation_Unit_of_water_feed = row["D3#5##Product#preparation_Unit#of#water#feed"].ToString();
										questionnaire.D3_6__Duration_of_usage = row["D3#6##Duration#of#usage"].ToString();
										#endregion

										#region C_4 Guidelines referred to bodyweight_pig_treatment purpose                  
										questionnaire.D4_1__Product_min = row["D4#1##Product#min"].ToString();
										questionnaire.D4_2__Product_max = row["D4#2##Product#max"].ToString();
										questionnaire.D4_3__Unit_of_product = D4_3__Unit_of_product;
										questionnaire.D4_4__Per_No__Kg_bodyweight_min = row["D4#4##Per#No##Kg#bodyweight_min"].ToString();
										questionnaire.D4_5__Per_No__Kg_bodyweight_max = row["D4#5##Per#No##Kg#bodyweight_max"].ToString();
										questionnaire.D4_6__Duration_of_usage = row["D4#6##Duration#of#usage"].ToString();
										#endregion

										#endregion

										#region E. Gia cầm
										var E1_2__Product_preparation_Unit_of_product = "";
										var E2_3__Unit_of_product = "";
										var E3_2__Product_preparation_Unit_of_product = "";
										var E4_3__Unit_of_product = "";
										foreach (var item in lsunitvolumn)
										{
											temp_unit = row["E1#2##Product#preparation_Unit#of#product"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";

											if (temp_unit == item)											
											{
												E1_2__Product_preparation_Unit_of_product = item;
											}

											temp_unit = row["E2#3##Unit#of#product"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";

											if (temp_unit == item)											
											{
												E2_3__Unit_of_product = item;
											}

											temp_unit = row["E3#2##Product#preparation_Unit#of#product"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";

											if (temp_unit == item)												
											{
												E3_2__Product_preparation_Unit_of_product = item;
											}

											temp_unit = row["E4#3##Unit#of#product"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";

											if (temp_unit == item)											
											{
												E4_3__Unit_of_product = item;
											}
										}
										#region C_1_ Product preparation (dilution) _pig_prevention purpose
										questionnaire.E1_1__Product_preparation__dilution__Product_amount = row["E1#1##Product#preparation##dilution#_Product#amount"].ToString();
										questionnaire.E1_2__Product_preparation_Unit_of_product = E1_2__Product_preparation_Unit_of_product;
										questionnaire.E1_3__Product_preparation_To_be_added_to__min_ = row["E1#3##Product#preparation_To#be#added#to##min#"].ToString();
										questionnaire.E1_4__Product_preparation_To_be_added_to__max_ = row["E1#4##Product#preparation_To#be#added#to##max#"].ToString();
										questionnaire.E1_5__Product_preparation_Unit_of_water_feed = row["E1#5##Product#preparation_Unit#of#water#feed"].ToString();
										questionnaire.E1_6__Duration_of_usage = row["E1#6##Duration#of#usage"].ToString();
										#endregion

										#region C.2 Guidelines referred to bodyweight_pig_prevention purpose
										questionnaire.E2_1__Product_min = row["E2#1##Product#min"].ToString();
										questionnaire.E2_2__Product_max = row["E2#2##Product#max"].ToString();
										questionnaire.E2_3__Unit_of_product = E2_3__Unit_of_product;
										questionnaire.E2_4__Per_No__Kg_bodyweight_min = row["E2#4##Per#No##Kg#bodyweight_min"].ToString();
										questionnaire.E2_5__Per_No__Kg_bodyweight_max = row["E2#5##Per#No##Kg#bodyweight_max"].ToString();
										questionnaire.E2_6__Duration_of_usage = row["E2#6##Duration#of#usage"].ToString();
										#endregion

										#region C_3 Product preparation (dilution) _pig_treatment purpose
										questionnaire.E3_1__Product_preparation__dilution__Product_amount = row["E3#1##Product#preparation##dilution#_Product#amount"].ToString();
										questionnaire.E3_2__Product_preparation_Unit_of_product = E3_2__Product_preparation_Unit_of_product;
										questionnaire.E3_3__Product_preparation_To_be_added_to__min_ = row["E3#3##Product#preparation_To#be#added#to##min#"].ToString();
										questionnaire.E3_4__Product_preparation_To_be_added_to__max_ = row["E3#4##Product#preparation_To#be#added#to##max#"].ToString();
										questionnaire.E3_5__Product_preparation_Unit_of_water_feed = row["E3#5##Product#preparation_Unit#of#water#feed"].ToString();
										questionnaire.E3_6__Duration_of_usage = row["E3#6##Duration#of#usage"].ToString();
										#endregion

										#region C_4 Guidelines referred to bodyweight_pig_treatment purpose                  
										questionnaire.E4_1__Product_min = row["E4#1##Product#min"].ToString();
										questionnaire.E4_2__Product_max = row["E4#2##Product#max"].ToString();
										questionnaire.E4_3__Unit_of_product = E4_3__Unit_of_product;
										questionnaire.E4_4_Per_No__Kg_bodyweight_min = row["E4#4#Per#No##Kg#bodyweight_min"].ToString();
										questionnaire.E4_5__Per_No__Kg_bodyweight_max = row["E4#5##Per#No##Kg#bodyweight_max"].ToString();
										questionnaire.E4_6__Duration_of_usage = row["E4#6##Duration#of#usage"].ToString();
										#endregion

										#endregion

										#region F.Further information

										//questionnaire.F_1__Source_of_information = row["F#1##Source#of#information"].ToString();
										//questionnaire.F_2__Picture_of_product = row["F#2##Picture#of#product"].ToString();
										//questionnaire.F3__Correction = row["F3##Correction"].ToString();
										//questionnaire.F_4__Person_in_charge = row["F#4##Person#in#charge"].ToString();
										//questionnaire.F_5__Working_time = row["F#5##Working#time"].ToString();
										questionnaire.F_6__Note = row["F#6##Note#"].ToString();

										#endregion

										#region G.Piglet
										var Piglet_1_2__Product_preparation_Unit_of_product = "";
										var Piglet_2_3__Unit_of_product = "";
										var Piglet_3_2__Product_preparation_Unit_of_product = "";
										var Piglet_4_3__Unit_of_product = "";
										foreach (var item in lsunitvolumn)
										{
											temp_unit = row["Piglet#1#2##Product#preparation_Unit#of#product"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";

											if (temp_unit == item)
											{
												Piglet_1_2__Product_preparation_Unit_of_product = item;
											}

											temp_unit = row["Piglet#2#3##Unit#of#product"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";

											if (temp_unit == item){
												Piglet_2_3__Unit_of_product = item;
											}

											temp_unit = row["Piglet#3#2##Product#preparation_Unit#of#product"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";

											if (temp_unit == item){
												Piglet_3_2__Product_preparation_Unit_of_product = item;
											}

											temp_unit = row["Piglet#4#3##Unit#of#product"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";

											if (temp_unit == item){
												Piglet_4_3__Unit_of_product = item;
											}
										}

										questionnaire.Piglet_1_1__Product_preparation__dilution__Product_amount = row["Piglet#1#1##Product#preparation##dilution#_Product#amount"].ToString();
										questionnaire.Piglet_1_2__Product_preparation_Unit_of_product = Piglet_1_2__Product_preparation_Unit_of_product;
										questionnaire.Piglet_1_3__Product_preparation_To_be_added_to__min_ = row["Piglet#1#3##Product#preparation_To#be#added#to##min#"].ToString();
										questionnaire.Piglet_1_4__Product_preparation_To_be_added_to__max_ = row["Piglet#1#4##Product#preparation_To#be#added#to##max#"].ToString();
										questionnaire.Piglet_1_5__Product_preparation_Unit_of_water_feed = row["Piglet#1#5##Product#preparation_Unit#of#water#feed"].ToString();
										questionnaire.Piglet_1_6__Duration_of_usage = row["Piglet#1#6##Duration#of#usage"].ToString();


										questionnaire.Piglet_2_1__Product_min = row["Piglet#2#1##Product#min"].ToString();
										questionnaire.Piglet_2_2__Product_max = row["Piglet#2#2##Product#max"].ToString();
										questionnaire.Piglet_2_3__Unit_of_product = Piglet_2_3__Unit_of_product;
										questionnaire.Piglet_2_4__Per_No__Kg_bodyweight_min = row["Piglet#2#4##Per#No##Kg#bodyweight_min"].ToString();
										questionnaire.Piglet_2_5__Per_No__Kg_bodyweight_max = row["Piglet#2#5##Per#No##Kg#bodyweight_max"].ToString();
										questionnaire.Piglet_2_6__Duration_of_usage = row["Piglet#2#6##Duration#of#usage"].ToString();


										questionnaire.Piglet_3_1__Product_preparation__dilution__Product_amount = row["Piglet#3#1##Product#preparation##dilution#_Product#amount"].ToString();
										questionnaire.Piglet_3_2__Product_preparation_Unit_of_product = Piglet_3_2__Product_preparation_Unit_of_product;
										questionnaire.Piglet_3_3__Product_preparation_To_be_added_to__min_ = row["Piglet#3#3##Product#preparation_To#be#added#to##min#"].ToString();
										questionnaire.Piglet_3_4__Product_preparation_To_be_added_to__max_ = row["Piglet#3#4##Product#preparation_To#be#added#to##max#"].ToString();
										questionnaire.Piglet_3_5__Product_preparation_Unit_of_water_feed = row["Piglet#3#5##Product#preparation_Unit#of#water#feed"].ToString();
										questionnaire.Piglet_3_6__Duration_of_usage = row["Piglet#3#6##Duration#of#usage"].ToString();



										questionnaire.Piglet_4_1__Product_min = row["Piglet#4#1##Product#min"].ToString();
										questionnaire.Piglet_4_2__Product_max = row["Piglet#4#2##Product#max"].ToString();
										questionnaire.Piglet_4_3__Unit_of_product = Piglet_4_3__Unit_of_product;
										questionnaire.Piglet_4_4_Per_No__Kg_bodyweight_min = row["Piglet#4#4#Per#No##Kg#bodyweight_min"].ToString();
										questionnaire.Piglet_4_5__Per_No__Kg_bodyweight_max = row["Piglet#4#5##Per#No##Kg#bodyweight_max"].ToString();
										questionnaire.Piglet_4_6__Duration_of_usage = row["Piglet#4#6##Duration#of#usage"].ToString();

										#endregion

										#region H.Buffalo
										var Buffalo_1_2__Product_preparation_Unit_of_product = "";
										var Buffalo_2_3__Unit_of_product = "";
										var Buffalo_3_2__Product_preparation_Unit_of_product = "";
										var Buffalo_4_3__Unit_of_product = "";
										var d = row["Buffalo#1#2##Product#preparation_Unit#of#product"].ToString();
										foreach (var item in lsunitvolumn)
										{
											temp_unit = row["Buffalo#1#2##Product#preparation_Unit#of#product"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";

											if (temp_unit == item){
												Buffalo_1_2__Product_preparation_Unit_of_product = item;
											}

											temp_unit = row["Buffalo#2#3##Unit#of#product"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";

											if (temp_unit == item){
												Buffalo_2_3__Unit_of_product = item;
											}

											temp_unit = row["Buffalo#3#2##Product#preparation_Unit#of#product"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";

											if (temp_unit == item){
												Buffalo_3_2__Product_preparation_Unit_of_product = item;
											}

											temp_unit = row["Buffalo#4#3##Unit#of#product"].ToString().Trim();
											if (temp_unit == "UI") temp_unit = "IU";

											if (temp_unit == item){
												Buffalo_4_3__Unit_of_product = item;
											}
										}

										questionnaire.Buffalo_1_1__Product_preparation__dilution__Product_amount = row["Buffalo#1#1##Product#preparation##dilution#_Product#amount"].ToString();
										questionnaire.Buffalo_1_2__Product_preparation_Unit_of_product = Buffalo_1_2__Product_preparation_Unit_of_product;
										questionnaire.Buffalo_1_3__Product_preparation_To_be_added_to__min_ = row["Buffalo#1#3##Product#preparation_To#be#added#to##min#"].ToString();
										questionnaire.Buffalo_1_4__Product_preparation_To_be_added_to__max_ = row["Buffalo#1#4##Product#preparation_To#be#added#to##max#"].ToString();
										questionnaire.Buffalo_1_5__Product_preparation_Unit_of_water_feed = row["Buffalo#1#5##Product#preparation_Unit#of#water#feed"].ToString();
										questionnaire.Buffalo_1_6__Duration_of_usage = row["Buffalo#1#6##Duration#of#usage"].ToString();


										questionnaire.Buffalo_2_1__Product_min = row["Buffalo#2#1##Product#min"].ToString();
										questionnaire.Buffalo_2_2__Product_max = row["Buffalo#2#2##Product#max"].ToString();
										questionnaire.Buffalo_2_3__Unit_of_product = Buffalo_2_3__Unit_of_product;
										questionnaire.Buffalo_2_4__Per_No__Kg_bodyweight_min = row["Buffalo#2#4##Per#No##Kg#bodyweight_min"].ToString();
										questionnaire.Buffalo_2_5__Per_No__Kg_bodyweight_max = row["Buffalo#2#5##Per#No##Kg#bodyweight_max"].ToString();
										questionnaire.Buffalo_2_6__Duration_of_usage = row["Buffalo#2#6##Duration#of#usage"].ToString();


										questionnaire.Buffalo_3_1__Product_preparation__dilution__Product_amount = row["Buffalo#3#1##Product#preparation##dilution#_Product#amount"].ToString();
										questionnaire.Buffalo_3_2__Product_preparation_Unit_of_product = Buffalo_3_2__Product_preparation_Unit_of_product;
										questionnaire.Buffalo_3_3__Product_preparation_To_be_added_to__min_ = row["Buffalo#3#3##Product#preparation_To#be#added#to##min#"].ToString();
										questionnaire.Buffalo_3_4__Product_preparation_To_be_added_to__max_ = row["Buffalo#3#4##Product#preparation_To#be#added#to##max#"].ToString();
										questionnaire.Buffalo_3_5__Product_preparation_Unit_of_water_feed = row["Buffalo#3#5##Product#preparation_Unit#of#water#feed"].ToString();
										questionnaire.Buffalo_3_6__Duration_of_usage = row["Buffalo#3#6##Duration#of#usage"].ToString();



										questionnaire.Buffalo_4_1__Product_min = row["Buffalo#4#1##Product#min"].ToString();
										questionnaire.Buffalo_4_2__Product_max = row["Buffalo#4#2##Product#max"].ToString();
										questionnaire.Buffalo_4_3__Unit_of_product = Buffalo_4_3__Unit_of_product;
										questionnaire.Buffalo_4_4_Per_No__Kg_bodyweight_min = row["Buffalo#4#4#Per#No##Kg#bodyweight_min"].ToString();
										questionnaire.Buffalo_4_5__Per_No__Kg_bodyweight_max = row["Buffalo#4#5##Per#No##Kg#bodyweight_max"].ToString();
										questionnaire.Buffalo_4_6__Duration_of_usage = row["Buffalo#4#6##Duration#of#usage"].ToString();

										#endregion

										questionnaire.State = State.APPROVE;
										questionnaires.Add(questionnaire);
									}
								}
							}
						}
					}

				}

				if (filePath.Length > 0)
				{
					DataSet ds = new DataSet();

					string ConnectionString = "";
					if (filePath.EndsWith(".xls"))
					{
						ConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", filePath);
					}
					else if (filePath.EndsWith(".xlsx"))
					{
						ConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", filePath);
					}
					using (OleDbConnection conn = new System.Data.OleDb.OleDbConnection(ConnectionString))
					{
						conn.Open();
						using (DataTable dtExcelSchema = conn.GetSchema("Tables"))
						{
							string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
							string query1 = "SELECT * FROM [" + sheetName + "$IC1:QJ12000]";
							OleDbDataAdapter adapter1 = new OleDbDataAdapter(query1, conn);
							//DataSet ds = new DataSet();
							adapter1.Fill(ds, "Items");
							if (ds.Tables.Count > 0)
							{
								if (ds.Tables[0].Rows.Count > 0)
								{
									for (int i = 0; i < ds.Tables[0].Rows.Count; i++)//ds.Tables[0].Rows.Count
									{
										DataRow row = ds.Tables[0].Rows[i];
										foreach (var questionnaire in questionnaires)
										{
											if (questionnaire.keyupload == (i + 1).ToString())
											{
												#region I.Cattle
												var Cattle_1_2__Product_preparation_Unit_of_product = "";
												var Cattle_2_3__Unit_of_product = "";
												var Cattle_3_2__Product_preparation_Unit_of_product = "";
												var Cattle_4_3__Unit_of_product = "";
												foreach (var item in lsunitvolumn)
												{

													string temp_unit = row["Cattle#1#2##Product#preparation_Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item){
														Cattle_1_2__Product_preparation_Unit_of_product = item;
													}

													temp_unit = row["Cattle#2#3##Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item){
														Cattle_2_3__Unit_of_product = item;
													}

													temp_unit = row["Cattle#3#2##Product#preparation_Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item){
														Cattle_3_2__Product_preparation_Unit_of_product = item;
													}

													temp_unit = row["Cattle#4#3##Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item){
														Cattle_4_3__Unit_of_product = item;
													}
												}

												questionnaire.Cattle_1_1__Product_preparation__dilution__Product_amount = row["Cattle#1#1##Product#preparation##dilution#_Product#amount"].ToString();
												questionnaire.Cattle_1_2__Product_preparation_Unit_of_product = Cattle_1_2__Product_preparation_Unit_of_product;
												questionnaire.Cattle_1_3__Product_preparation_To_be_added_to__min_ = row["Cattle#1#3##Product#preparation_To#be#added#to##min#"].ToString();
												questionnaire.Cattle_1_4__Product_preparation_To_be_added_to__max_ = row["Cattle#1#4##Product#preparation_To#be#added#to##max#"].ToString();
												questionnaire.Cattle_1_5__Product_preparation_Unit_of_water_feed = row["Cattle#1#5##Product#preparation_Unit#of#water#feed"].ToString();
												questionnaire.Cattle_1_6__Duration_of_usages = row["Cattle#1#6##Duration#of#usage"].ToString();


												questionnaire.Cattle_2_1__Product_min = row["Cattle#2#1##Product#min"].ToString();
												questionnaire.Cattle_2_2__Product_max = row["Cattle#2#2##Product#max"].ToString();
												questionnaire.Cattle_2_3__Unit_of_product = Cattle_2_3__Unit_of_product;
												questionnaire.Cattle_2_4__Per_No__Kg_bodyweight_min = row["Cattle#2#4##Per#No##Kg#bodyweight_min"].ToString();
												questionnaire.Cattle_2_5__Per_No__Kg_bodyweight_max = row["Cattle#2#5##Per#No##Kg#bodyweight_max"].ToString();
												questionnaire.Cattle_2_6__Duration_of_usage = row["Cattle#2#6##Duration#of#usage"].ToString();


												questionnaire.Cattle_3_1__Product_preparation__dilution__Product_amount = row["Cattle#3#1##Product#preparation##dilution#_Product#amount"].ToString();
												questionnaire.Cattle_3_2__Product_preparation_Unit_of_product = Cattle_3_2__Product_preparation_Unit_of_product;
												questionnaire.Cattle_3_3__Product_preparation_To_be_added_to__min_ = row["Cattle#3#3##Product#preparation_To#be#added#to##min#"].ToString();
												questionnaire.Cattle_3_4__Product_preparation_To_be_added_to__max_ = row["Cattle#3#4##Product#preparation_To#be#added#to##max#"].ToString();
												questionnaire.Cattle_3_5__Product_preparation_Unit_of_water_feed = row["Cattle#3#5##Product#preparation_Unit#of#water#feed"].ToString();
												questionnaire.Cattle_3_6__Duration_of_usage = row["Cattle#3#6##Duration#of#usage"].ToString();



												questionnaire.Cattle_4_1__Product_min = row["Cattle#4#1##Product#min"].ToString();
												questionnaire.Cattle_4_2__Product_max = row["Cattle#4#2##Product#max"].ToString();
												questionnaire.Cattle_4_3__Unit_of_product = Cattle_4_3__Unit_of_product;
												questionnaire.Cattle_4_4_Per_No__Kg_bodyweight_min = row["Cattle#4#4#Per#No##Kg#bodyweight_min"].ToString();
												questionnaire.Cattle_4_5__Per_No__Kg_bodyweight_max = row["Cattle#4#5##Per#No##Kg#bodyweight_max"].ToString();
												questionnaire.Cattle_4_6__Duration_of_usage = row["Cattle#4#6##Duration#of#usage"].ToString();

												#endregion

												#region J.Goat
												var Goat_1_2__Product_preparation_Unit_of_product = "";
												var Goat_2_3__Unit_of_product = "";
												var Goat_3_2__Product_preparation_Unit_of_product = "";
												var Goat_4_3__Unit_of_product = "";
												foreach (var item in lsunitvolumn)
												{
													string temp_unit = row["Goat#1#2##Product#preparation_Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)													
													{
														Goat_1_2__Product_preparation_Unit_of_product = item;
													}
													temp_unit = row["Goat#2#3##Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)													
													{
														Goat_2_3__Unit_of_product = item;
													}
													temp_unit = row["Goat#3#2##Product#preparation_Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)														
													{
														Goat_3_2__Product_preparation_Unit_of_product = item;
													}
													temp_unit = row["Goat#4#3##Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item){
														Goat_4_3__Unit_of_product = item;
													}
												}
												questionnaire.Goat_1_1__Product_preparation__dilution__Product_amount = row["Goat#1#1##Product#preparation##dilution#_Product#amount"].ToString();
												questionnaire.Goat_1_2__Product_preparation_Unit_of_product = Goat_1_2__Product_preparation_Unit_of_product;
												questionnaire.Goat_1_3__Product_preparation_To_be_added_to__min_ = row["Goat#1#3##Product#preparation_To#be#added#to##min#"].ToString();
												questionnaire.Goat_1_4__Product_preparation_To_be_added_to__max_ = row["Goat#1#4##Product#preparation_To#be#added#to##max#"].ToString();
												questionnaire.Goat_1_5__Product_preparation_Unit_of_water_feed = row["Goat#1#5##Product#preparation_Unit#of#water#feed"].ToString();
												questionnaire.Goat_1_6__Duration_of_usage = row["Goat#1#6##Duration#of#usage"].ToString();


												questionnaire.Goat_2_1__Product_min = row["Goat#2#1##Product#min"].ToString();
												questionnaire.Goat_2_2__Product_max = row["Goat#2#2##Product#max"].ToString();
												questionnaire.Goat_2_3__Unit_of_product = Goat_2_3__Unit_of_product;
												questionnaire.Goat_2_4__Per_No__Kg_bodyweight_min = row["Goat#2#4##Per#No##Kg#bodyweight_min"].ToString();
												questionnaire.Goat_2_5__Per_No__Kg_bodyweight_max = row["Goat#2#5##Per#No##Kg#bodyweight_max"].ToString();
												questionnaire.Goat_2_6__Duration_of_usage = row["Goat#2#6##Duration#of#usage"].ToString();


												questionnaire.Goat_3_1__Product_preparation__dilution__Product_amount = row["Goat#3#1##Product#preparation##dilution#_Product#amount"].ToString();
												questionnaire.Goat_3_2__Product_preparation_Unit_of_product = Goat_3_2__Product_preparation_Unit_of_product;
												questionnaire.Goat_3_3__Product_preparation_To_be_added_to__min_ = row["Goat#3#3##Product#preparation_To#be#added#to##min#"].ToString();
												questionnaire.Goat_3_4__Product_preparation_To_be_added_to__max_ = row["Goat#3#4##Product#preparation_To#be#added#to##max#"].ToString();
												questionnaire.Goat_3_5__Product_preparation_Unit_of_water_feed = row["Goat#3#5##Product#preparation_Unit#of#water#feed"].ToString();
												questionnaire.Goat_3_6__Duration_of_usage = row["Goat#3#6##Duration#of#usage"].ToString();



												questionnaire.Goat_4_1__Product_min = row["Goat#4#1##Product#min"].ToString();
												questionnaire.Goat_4_2__Product_max = row["Goat#4#2##Product#max"].ToString();
												questionnaire.Goat_4_3__Unit_of_product = Goat_4_3__Unit_of_product;
												questionnaire.Goat_4_4_Per_No__Kg_bodyweight_min = row["Goat#4#4#Per#No##Kg#bodyweight_min"].ToString();
												questionnaire.Goat_4_5__Per_No__Kg_bodyweight_max = row["Goat#4#5##Per#No##Kg#bodyweight_max"].ToString();
												questionnaire.Goat_4_6__Duration_of_usage = row["Goat#4#6##Duration#of#usage"].ToString();

												#endregion

												#region K.Sheep
												var Sheep_1_2__Product_preparation_Unit_of_product = "";
												var Sheep_2_3__Unit_of_product = "";
												var Sheep_3_2__Product_preparation_Unit_of_product = "";
												var Sheep_4_3__Unit_of_product = "";
												foreach (var item in lsunitvolumn)
												{
													string temp_unit = row["Sheep#1#2##Product#preparation_Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)
													{
														Sheep_1_2__Product_preparation_Unit_of_product = item;
													}

													temp_unit = row["Sheep#2#3##Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)
													{
														Sheep_2_3__Unit_of_product = item;
													}

													temp_unit = row["Sheep#3#2##Product#preparation_Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)
													{
														Sheep_3_2__Product_preparation_Unit_of_product = item;
													}

													temp_unit = row["Sheep#4#3##Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)
													{
														Sheep_4_3__Unit_of_product = item;
													}
												}
												questionnaire.Sheep_1_1__Product_preparation__dilution__Product_amount = row["Sheep#1#1##Product#preparation##dilution#_Product#amount"].ToString();
												questionnaire.Sheep_1_2__Product_preparation_Unit_of_product = Sheep_1_2__Product_preparation_Unit_of_product;
												questionnaire.Sheep_1_3__Product_preparation_To_be_added_to__min_ = row["Sheep#1#3##Product#preparation_To#be#added#to##min#"].ToString();
												questionnaire.Sheep_1_4__Product_preparation_To_be_added_to__max_ = row["Sheep#1#4##Product#preparation_To#be#added#to##max#"].ToString();
												questionnaire.Sheep_1_5__Product_preparation_Unit_of_water_feed = row["Sheep#1#5##Product#preparation_Unit#of#water#feed"].ToString();
												questionnaire.Sheep_1_6__Duration_of_usage = row["Sheep#1#6##Duration#of#usage"].ToString();


												questionnaire.Sheep_2_1__Product_min = row["Sheep#2#1##Product#min"].ToString();
												questionnaire.Sheep_2_2__Product_max = row["Sheep#2#2##Product#max"].ToString();
												questionnaire.Sheep_2_3__Unit_of_product = Sheep_2_3__Unit_of_product;
												questionnaire.Sheep_2_4__Per_No__Kg_bodyweight_min = row["Sheep#2#4##Per#No##Kg#bodyweight_min"].ToString();
												questionnaire.Sheep_2_5__Per_No__Kg_bodyweight_max = row["Sheep#2#5##Per#No##Kg#bodyweight_max"].ToString();
												questionnaire.Sheep_2_6__Duration_of_usage = row["Sheep#2#6##Duration#of#usage"].ToString();


												questionnaire.Sheep_3_1__Product_preparation__dilution__Product_amount = row["Sheep#3#1##Product#preparation##dilution#_Product#amount"].ToString();
												questionnaire.Sheep_3_2__Product_preparation_Unit_of_product = Sheep_3_2__Product_preparation_Unit_of_product;
												questionnaire.Sheep_3_3__Product_preparation_To_be_added_to__min_ = row["Sheep#3#3##Product#preparation_To#be#added#to##min#"].ToString();
												questionnaire.Sheep_3_4__Product_preparation_To_be_added_to__max_ = row["Sheep#3#4##Product#preparation_To#be#added#to##max#"].ToString();
												questionnaire.Sheep_3_5__Product_preparation_Unit_of_water_feed = row["Sheep#3#5##Product#preparation_Unit#of#water#feed"].ToString();
												questionnaire.Sheep_3_6__Duration_of_usage = row["Sheep#3#6##Duration#of#usage"].ToString();



												questionnaire.Sheep_4_1__Product_min = row["Sheep#4#1##Product#min"].ToString();
												questionnaire.Sheep_4_2__Product_max = row["Sheep#4#2##Product#max"].ToString();
												questionnaire.Sheep_4_3__Unit_of_product = Sheep_4_3__Unit_of_product;
												questionnaire.Sheep_4_4_Per_No__Kg_bodyweight_min = row["Sheep#4#4#Per#No##Kg#bodyweight_min"].ToString();
												questionnaire.Sheep_4_5__Per_No__Kg_bodyweight_max = row["Sheep#4#5##Per#No##Kg#bodyweight_max"].ToString();
												questionnaire.Sheep_4_6__Duration_of_usage = row["Sheep#4#6##Duration#of#usage"].ToString();

												#endregion

												#region L.Horse
												var Horse_1_2__Product_preparation_Unit_of_product = "";
												var Horse_2_3__Unit_of_product = "";
												var Horse_3_2__Product_preparation_Unit_of_product = "";
												var Horse_4_3__Unit_of_product = "";
												foreach (var item in lsunitvolumn)
												{
													string temp_unit = row["Horse#1#2##Product#preparation_Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)												
													{
														Horse_1_2__Product_preparation_Unit_of_product = item;
													}

													temp_unit = row["Horse#2#3##Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";
													if (temp_unit == item)
													{
														Horse_2_3__Unit_of_product = item;
													}													
													temp_unit = row["Horse#3#2##Product#preparation_Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";
													if (temp_unit == item)
													{
														Horse_3_2__Product_preparation_Unit_of_product = item;
													}
													temp_unit = row["Horse#4#3##Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";
													if (temp_unit == item)
													{
														Horse_4_3__Unit_of_product = item;
													}
												}
												questionnaire.Horse_1_1__Product_preparation__dilution__Product_amount = row["Horse#1#1##Product#preparation##dilution#_Product#amount"].ToString();
												questionnaire.Horse_1_2__Product_preparation_Unit_of_product = Horse_1_2__Product_preparation_Unit_of_product;
												questionnaire.Horse_1_3__Product_preparation_To_be_added_to__min_ = row["Horse#1#3##Product#preparation_To#be#added#to##min#"].ToString();
												questionnaire.Horse_1_4__Product_preparation_To_be_added_to__max_ = row["Horse#1#4##Product#preparation_To#be#added#to##max#"].ToString();
												questionnaire.Horse_1_5__Product_preparation_Unit_of_water_feed = row["Horse#1#5##Product#preparation_Unit#of#water#feed"].ToString();
												questionnaire.Horse_1_6__Duration_of_usage = row["Horse#1#6##Duration#of#usage"].ToString();


												questionnaire.Horse_2_1__Product_min = row["Horse#2#1##Product#min"].ToString();
												questionnaire.Horse_2_2__Product_max = row["Horse#2#2##Product#max"].ToString();
												questionnaire.Horse_2_3__Unit_of_product = Horse_2_3__Unit_of_product;
												questionnaire.Horse_2_4__Per_No__Kg_bodyweight_min = row["Horse#2#4##Per#No##Kg#bodyweight_min"].ToString();
												questionnaire.Horse_2_5__Per_No__Kg_bodyweight_max = row["Horse#2#5##Per#No##Kg#bodyweight_max"].ToString();
												questionnaire.Horse_2_6__Duration_of_usage = row["Horse#2#6##Duration#of#usage"].ToString();


												questionnaire.Horse_3_1__Product_preparation__dilution__Product_amount = row["Horse#3#1##Product#preparation##dilution#_Product#amount"].ToString();
												questionnaire.Horse_3_2__Product_preparation_Unit_of_product = Horse_3_2__Product_preparation_Unit_of_product;
												questionnaire.Horse_3_3__Product_preparation_To_be_added_to__min_ = row["Horse#3#3##Product#preparation_To#be#added#to##min#"].ToString();
												questionnaire.Horse_3_4__Product_preparation_To_be_added_to__max_ = row["Horse#3#4##Product#preparation_To#be#added#to##max#"].ToString();
												questionnaire.Horse_3_5__Product_preparation_Unit_of_water_feed = row["Horse#3#5##Product#preparation_Unit#of#water#feed"].ToString();
												questionnaire.Horse_3_6__Duration_of_usage = row["Horse#3#6##Duration#of#usage"].ToString();



												questionnaire.Horse_4_1__Product_min = row["Horse#4#1##Product#min"].ToString();
												questionnaire.Horse_4_2__Product_max = row["Horse#4#2##Product#max"].ToString();
												questionnaire.Horse_4_3__Unit_of_product = Horse_4_3__Unit_of_product;
												questionnaire.Horse_4_4_Per_No__Kg_bodyweight_min = row["Horse#4#4#Per#No##Kg#bodyweight_min"].ToString();
												questionnaire.Horse_4_5__Per_No__Kg_bodyweight_max = row["Horse#4#5##Per#No##Kg#bodyweight_max"].ToString();
												questionnaire.Horse_4_6__Duration_of_usage = row["Horse#4#6##Duration#of#usage"].ToString();

												#endregion

												#region M.Chicken
												var Chicken_1_2__Product_preparation_Unit_of_product = "";
												var Chicken_2_3__Unit_of_product = "";
												var Chicken_3_2__Product_preparation_Unit_of_product = "";
												var Chicken_4_3__Unit_of_product = "";
												foreach (var item in lsunitvolumn)
												{
													string temp_unit = row["Chicken#1#2##Product#preparation_Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)
													{
														Chicken_1_2__Product_preparation_Unit_of_product = item;
													}
													temp_unit = row["Chicken#2#3##Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)
													{
														Chicken_2_3__Unit_of_product = item;
													}

													temp_unit = row["Chicken#3#2##Product#preparation_Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";
													if (temp_unit == item)
													{
														Chicken_3_2__Product_preparation_Unit_of_product = item;
													}

													temp_unit = row["Chicken#4#3##Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";
													if (temp_unit == item)
													{
														Chicken_4_3__Unit_of_product = item;
													}
												}
												questionnaire.Chicken_1_1__Product_preparation__dilution__Product_amount = row["Chicken#1#1##Product#preparation##dilution#_Product#amount"].ToString();
												questionnaire.Chicken_1_2__Product_preparation_Unit_of_product = Chicken_1_2__Product_preparation_Unit_of_product;
												questionnaire.Chicken_1_3__Product_preparation_To_be_added_to__min_ = row["Chicken#1#3##Product#preparation_To#be#added#to##min#"].ToString();
												questionnaire.Chicken_1_4__Product_preparation_To_be_added_to__max_ = row["Chicken#1#4##Product#preparation_To#be#added#to##max#"].ToString();
												questionnaire.Chicken_1_5__Product_preparation_Unit_of_water_feed = row["Chicken#1#5##Product#preparation_Unit#of#water#feed"].ToString();
												questionnaire.Chicken_1_6__Duration_of_usage = row["Chicken#1#6##Duration#of#usage"].ToString();


												questionnaire.Chicken_2_1__Product_min = row["Chicken#2#1##Product#min"].ToString();
												questionnaire.Chicken_2_2__Product_max = row["Chicken#2#2##Product#max"].ToString();
												questionnaire.Chicken_2_3__Unit_of_product = Chicken_2_3__Unit_of_product;
												questionnaire.Chicken_2_4__Per_No__Kg_bodyweight_min = row["Chicken#2#4##Per#No##Kg#bodyweight_min"].ToString();
												questionnaire.Chicken_2_5__Per_No__Kg_bodyweight_max = row["Chicken#2#5##Per#No##Kg#bodyweight_max"].ToString();
												questionnaire.Chicken_2_6__Duration_of_usage = row["Chicken#2#6##Duration#of#usage"].ToString();


												questionnaire.Chicken_3_1__Product_preparation__dilution__Product_amount = row["Chicken#3#1##Product#preparation##dilution#_Product#amount"].ToString();
												questionnaire.Chicken_3_2__Product_preparation_Unit_of_product = Chicken_3_2__Product_preparation_Unit_of_product;
												questionnaire.Chicken_3_3__Product_preparation_To_be_added_to__min_ = row["Chicken#3#3##Product#preparation_To#be#added#to##min#"].ToString();
												questionnaire.Chicken_3_4__Product_preparation_To_be_added_to__max_ = row["Chicken#3#4##Product#preparation_To#be#added#to##max#"].ToString();
												questionnaire.Chicken_3_5__Product_preparation_Unit_of_water_feed = row["Chicken#3#5##Product#preparation_Unit#of#water#feed"].ToString();
												questionnaire.Chicken_3_6__Duration_of_usage = row["Chicken#3#6##Duration#of#usage"].ToString();



												questionnaire.Chicken_4_1__Product_min = row["Chicken#4#1##Product#min"].ToString();
												questionnaire.Chicken_4_2__Product_max = row["Chicken#4#2##Product#max"].ToString();
												questionnaire.Chicken_4_3__Unit_of_product = Chicken_4_3__Unit_of_product;
												questionnaire.Chicken_4_4_Per_No__Kg_bodyweight_min = row["Chicken#4#4#Per#No##Kg#bodyweight_min"].ToString();
												questionnaire.Chicken_4_5__Per_No__Kg_bodyweight_max = row["Chicken#4#5##Per#No##Kg#bodyweight_max"].ToString();
												questionnaire.Chicken_4_6__Duration_of_usage = row["Chicken#4#6##Duration#of#usage"].ToString();

												#endregion

												#region N.Duck
												var Duck_1_2__Product_preparation_Unit_of_product = "";
												var Duck_2_3__Unit_of_product = "";
												var Duck_3_2__Product_preparation_Unit_of_product = "";
												var Duck_4_3__Unit_of_product = "";
												foreach (var item in lsunitvolumn)
												{
													string temp_unit = row["Duck#1#2##Product#preparation_Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";
													if (temp_unit == item)
													{
														Duck_1_2__Product_preparation_Unit_of_product = item;
													}

													temp_unit = row["Duck#2#3##Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";
													if (temp_unit == item)
													{
														Duck_2_3__Unit_of_product = item;
													}

													temp_unit = row["Duck#3#2##Product#preparation_Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";
													if (temp_unit == item)
													{
														Duck_3_2__Product_preparation_Unit_of_product = item;
													}

													temp_unit = row["Duck#4#3##Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";
													if (temp_unit == item)
													{
														Duck_4_3__Unit_of_product = item;
													}
												}

												questionnaire.Duck_1_1__Product_preparation__dilution__Product_amount = row["Duck#1#1##Product#preparation##dilution#_Product#amount"].ToString();
												questionnaire.Duck_1_2__Product_preparation_Unit_of_product = Duck_1_2__Product_preparation_Unit_of_product;
												questionnaire.Duck_1_3__Product_preparation_To_be_added_to__min_ = row["Duck#1#3##Product#preparation_To#be#added#to##min#"].ToString();
												questionnaire.Duck_1_4__Product_preparation_To_be_added_to__max_ = row["Duck#1#4##Product#preparation_To#be#added#to##max#"].ToString();
												questionnaire.Duck_1_5__Product_preparation_Unit_of_water_feed = row["Duck#1#5##Product#preparation_Unit#of#water#feed"].ToString();
												questionnaire.Duck_1_6__Duration_of_usage = row["Duck#1#6##Duration#of#usage"].ToString();


												questionnaire.Duck_2_1__Product_min = row["Duck#2#1##Product#min"].ToString();
												questionnaire.Duck_2_2__Product_max = row["Duck#2#2##Product#max"].ToString();
												questionnaire.Duck_2_3__Unit_of_product = Duck_2_3__Unit_of_product;
												questionnaire.Duck_2_4__Per_No__Kg_bodyweight_min = row["Duck#2#4##Per#No##Kg#bodyweight_min"].ToString();
												questionnaire.Duck_2_5__Per_No__Kg_bodyweight_max = row["Duck#2#5##Per#No##Kg#bodyweight_max"].ToString();
												questionnaire.Duck_2_6__Duration_of_usage = row["Duck#2#6##Duration#of#usage"].ToString();


												questionnaire.Duck_3_1__Product_preparation__dilution__Product_amount = row["Duck#3#1##Product#preparation##dilution#_Product#amount"].ToString();
												questionnaire.Duck_3_2__Product_preparation_Unit_of_product = Duck_3_2__Product_preparation_Unit_of_product;
												questionnaire.Duck_3_3__Product_preparation_To_be_added_to__min_ = row["Duck#3#3##Product#preparation_To#be#added#to##min#"].ToString();
												questionnaire.Duck_3_4__Product_preparation_To_be_added_to__max_ = row["Duck#3#4##Product#preparation_To#be#added#to##max#"].ToString();
												questionnaire.Duck_3_5__Product_preparation_Unit_of_water_feed = row["Duck#3#5##Product#preparation_Unit#of#water#feed"].ToString();
												questionnaire.Duck_3_6__Duration_of_usage = row["Duck#3#6##Duration#of#usage"].ToString();



												questionnaire.Duck_4_1__Product_min = row["Duck#4#1##Product#min"].ToString();
												questionnaire.Duck_4_2__Product_max = row["Duck#4#2##Product#max"].ToString();
												questionnaire.Duck_4_3__Unit_of_product = Duck_4_3__Unit_of_product;
												questionnaire.Duck_4_4_Per_No__Kg_bodyweight_min = row["Duck#4#4#Per#No##Kg#bodyweight_min"].ToString();
												questionnaire.Duck_4_5__Per_No__Kg_bodyweight_max = row["Duck#4#5##Per#No##Kg#bodyweight_max"].ToString();
												questionnaire.Duck_4_6__Duration_of_usage = row["Duck#4#6##Duration#of#usage"].ToString();

												#endregion

												#region O.Muscovy_Duck
												var Muscovy_Duck_1_2__Product_preparation_Unit_of_product = "";
												var Muscovy_Duck_2_3__Unit_of_product = "";
												var Muscovy_Duck_3_2__Product_preparation_Unit_of_product = "";
												var Muscovy_Duck_4_3__Unit_of_product = "";
												foreach (var item in lsunitvolumn)
												{

													string temp_unit = row["Muscovy#Duck#1#2##Product#preparation_Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)
													{
														Muscovy_Duck_1_2__Product_preparation_Unit_of_product = item;
													}

													temp_unit = row["Muscovy#Duck#2#3##Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)													
													{
														Muscovy_Duck_2_3__Unit_of_product = item;
													}

													temp_unit = row["Muscovy#Duck#3#2##Product#preparation_Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)													
													{
														Muscovy_Duck_3_2__Product_preparation_Unit_of_product = item;
													}

													temp_unit = row["Muscovy#Duck#4#3##Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)													
													{
														Muscovy_Duck_4_3__Unit_of_product = item;
													}
												}

												questionnaire.Muscovy_Duck_1_1__Product_preparation__dilution__Product_amount = row["Muscovy#Duck#1#1##Product#preparation##dilution#_Product#amount"].ToString();
												questionnaire.Muscovy_Duck_1_2__Product_preparation_Unit_of_product = Muscovy_Duck_1_2__Product_preparation_Unit_of_product;
												questionnaire.Muscovy_Duck_1_3__Product_preparation_To_be_added_to__min_ = row["Muscovy#Duck#1#3##Product#preparation_To#be#added#to##min#"].ToString();
												questionnaire.Muscovy_Duck_1_4__Product_preparation_To_be_added_to__max_ = row["Muscovy#Duck#1#4##Product#preparation_To#be#added#to##max#"].ToString();
												questionnaire.Muscovy_Duck_1_5__Product_preparation_Unit_of_water_feed = row["Muscovy#Duck#1#5##Product#preparation_Unit#of#water#feed"].ToString();
												questionnaire.Muscovy_Duck_1_6__Duration_of_usage = row["Muscovy#Duck#1#6##Duration#of#usage"].ToString();


												questionnaire.Muscovy_Duck_2_1__Product_min = row["Muscovy#Duck#2#1##Product#min"].ToString();
												questionnaire.Muscovy_Duck_2_2__Product_max = row["Muscovy#Duck#2#2##Product#max"].ToString();
												questionnaire.Muscovy_Duck_2_3__Unit_of_product = Muscovy_Duck_2_3__Unit_of_product;
												questionnaire.Muscovy_Duck_2_4__Per_No__Kg_bodyweight_min = row["Muscovy#Duck#2#4##Per#No##Kg#bodyweight_min"].ToString();
												questionnaire.Muscovy_Duck_2_5__Per_No__Kg_bodyweight_max = row["Muscovy#Duck#2#5##Per#No##Kg#bodyweight_max"].ToString();
												questionnaire.Muscovy_Duck_2_6__Duration_of_usage = row["Muscovy#Duck#2#6##Duration#of#usage"].ToString();


												questionnaire.Muscovy_Duck_3_1__Product_preparation__dilution__Product_amount = row["Muscovy#Duck#3#1##Product#preparation##dilution#_Product#amount"].ToString();
												questionnaire.Muscovy_Duck_3_2__Product_preparation_Unit_of_product = Muscovy_Duck_3_2__Product_preparation_Unit_of_product;
												questionnaire.Muscovy_Duck_3_3__Product_preparation_To_be_added_to__min_ = row["Muscovy#Duck#3#3##Product#preparation_To#be#added#to##min#"].ToString();
												questionnaire.Muscovy_Duck_3_4__Product_preparation_To_be_added_to__max_ = row["Muscovy#Duck#3#4##Product#preparation_To#be#added#to##max#"].ToString();
												questionnaire.Muscovy_Duck_3_5__Product_preparation_Unit_of_water_feed = row["Muscovy#Duck#3#5##Product#preparation_Unit#of#water#feed"].ToString();
												questionnaire.Muscovy_Duck_3_6__Duration_of_usage = row["Muscovy#Duck#3#6##Duration#of#usage"].ToString();



												questionnaire.Muscovy_Duck_4_1__Product_min = row["Muscovy#Duck#4#1##Product#min"].ToString();
												questionnaire.Muscovy_Duck_4_2__Product_max = row["Muscovy#Duck#4#2##Product#max"].ToString();
												questionnaire.Muscovy_Duck_4_3__Unit_of_product = Muscovy_Duck_4_3__Unit_of_product;
												questionnaire.Muscovy_Duck_4_4_Per_No__Kg_bodyweight_min = row["Muscovy#Duck#4#4#Per#No##Kg#bodyweight_min"].ToString();
												questionnaire.Muscovy_Duck_4_5__Per_No__Kg_bodyweight_max = row["Muscovy#Duck#4#5##Per#No##Kg#bodyweight_max"].ToString();
												questionnaire.Muscovy_Duck_4_6__Duration_of_usage = row["Muscovy#Duck#4#6##Duration#of#usage"].ToString();

												#endregion

												#region P.Quail
												var Quail_1_2__Product_preparation_Unit_of_product = "";
												var Quail_2_3__Unit_of_product = "";
												var Quail_3_2__Product_preparation_Unit_of_product = "";
												var Quail_4_3__Unit_of_product = "";
												foreach (var item in lsunitvolumn)
												{

													string temp_unit = row["Quail#1#2##Product#preparation_Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)
													{
														Quail_1_2__Product_preparation_Unit_of_product = item;
													}

													temp_unit = row["Quail#2#3##Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)
													{
														Quail_2_3__Unit_of_product = item;
													}

													temp_unit = row["Quail#3#2##Product#preparation_Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)
													{
														Quail_3_2__Product_preparation_Unit_of_product = item;
													}

													temp_unit = row["Quail#4#3##Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)
													{
														Quail_4_3__Unit_of_product = item;
													}
												}

												questionnaire.Quail_1_1__Product_preparation__dilution__Product_amount = row["Quail#1#1##Product#preparation##dilution#_Product#amount"].ToString();
												questionnaire.Quail_1_2__Product_preparation_Unit_of_product = Quail_1_2__Product_preparation_Unit_of_product;
												questionnaire.Quail_1_3__Product_preparation_To_be_added_to__min_ = row["Quail#1#3##Product#preparation_To#be#added#to##min#"].ToString();
												questionnaire.Quail_1_4__Product_preparation_To_be_added_to__max_ = row["Quail#1#4##Product#preparation_To#be#added#to##max#"].ToString();
												questionnaire.Quail_1_5__Product_preparation_Unit_of_water_feed = row["Quail#1#5##Product#preparation_Unit#of#water#feed"].ToString();
												questionnaire.Quail_1_6__Duration_of_usage = row["Quail#1#6##Duration#of#usage"].ToString();


												questionnaire.Quail_2_1__Product_min = row["Quail#2#1##Product#min"].ToString();
												questionnaire.Quail_2_2__Product_max = row["Quail#2#2##Product#max"].ToString();
												questionnaire.Quail_2_3__Unit_of_product = Quail_2_3__Unit_of_product;
												questionnaire.Quail_2_4__Per_No__Kg_bodyweight_min = row["Quail#2#4##Per#No##Kg#bodyweight_min"].ToString();
												questionnaire.Quail_2_5__Per_No__Kg_bodyweight_max = row["Quail#2#5##Per#No##Kg#bodyweight_max"].ToString();
												questionnaire.Quail_2_6__Duration_of_usage = row["Quail#2#6##Duration#of#usage"].ToString();


												questionnaire.Quail_3_1__Product_preparation__dilution__Product_amount = row["Quail#3#1##Product#preparation##dilution#_Product#amount"].ToString();
												questionnaire.Quail_3_2__Product_preparation_Unit_of_product = Quail_3_2__Product_preparation_Unit_of_product;
												questionnaire.Quail_3_3__Product_preparation_To_be_added_to__min_ = row["Quail#3#3##Product#preparation_To#be#added#to##min#"].ToString();
												questionnaire.Quail_3_4__Product_preparation_To_be_added_to__max_ = row["Quail#3#4##Product#preparation_To#be#added#to##max#"].ToString();
												questionnaire.Quail_3_5__Product_preparation_Unit_of_water_feed = row["Quail#3#5##Product#preparation_Unit#of#water#feed"].ToString();
												questionnaire.Quail_3_6__Duration_of_usage = row["Quail#3#6##Duration#of#usage"].ToString();



												questionnaire.Quail_4_1__Product_min = row["Quail#4#1##Product#min"].ToString();
												questionnaire.Quail_4_2__Product_max = row["Quail#4#2##Product#max"].ToString();
												questionnaire.Quail_4_3__Unit_of_product = Quail_4_3__Unit_of_product;
												questionnaire.Quail_4_4_Per_No__Kg_bodyweight_min = row["Quail#4#4#Per#No##Kg#bodyweight_min"].ToString();
												questionnaire.Quail_4_5__Per_No__Kg_bodyweight_max = row["Quail#4#5##Per#No##Kg#bodyweight_max"].ToString();
												questionnaire.Quail_4_6__Duration_of_usage = row["Quail#4#6##Duration#of#usage"].ToString();

												#endregion

												#region Q.Goose
												var Goose_1_2__Product_preparation_Unit_of_product = "";
												var Goose_2_3__Unit_of_product = "";
												var Goose_3_2__Product_preparation_Unit_of_product = "";
												var Goose_4_3__Unit_of_product = "";
												foreach (var item in lsunitvolumn)
												{
													string temp_unit = row["Goose#1#2##Product#preparation_Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)
													{
														Goose_1_2__Product_preparation_Unit_of_product = item;
													}
													temp_unit = row["Goose#2#3##Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)
													{
														Goose_2_3__Unit_of_product = item;
													}
													temp_unit = row["Goose#3#2##Product#preparation_Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)
													{
														Goose_3_2__Product_preparation_Unit_of_product = item;
													}
													temp_unit = row["Goose#4#3##Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)
													{
														Goose_4_3__Unit_of_product = item;
													}
												}

												questionnaire.Goose_1_1__Product_preparation__dilution__Product_amount = row["Goose#1#1##Product#preparation##dilution#_Product#amount"].ToString();
												questionnaire.Goose_1_2__Product_preparation_Unit_of_product = Goose_1_2__Product_preparation_Unit_of_product;
												questionnaire.Goose_1_3__Product_preparation_To_be_added_to__min_ = row["Goose#1#3##Product#preparation_To#be#added#to##min#"].ToString();
												questionnaire.Goose_1_4__Product_preparation_To_be_added_to__max_ = row["Goose#1#4##Product#preparation_To#be#added#to##max#"].ToString();
												questionnaire.Goose_1_5__Product_preparation_Unit_of_water_feed = row["Goose#1#5##Product#preparation_Unit#of#water#feed"].ToString();
												questionnaire.Goose_1_6__Duration_of_usage = row["Goose#1#6##Duration#of#usage"].ToString();


												questionnaire.Goose_2_1__Product_min = row["Goose#2#1##Product#min"].ToString();
												questionnaire.Goose_2_2__Product_max = row["Goose#2#2##Product#max"].ToString();
												questionnaire.Goose_2_3__Unit_of_product = Goose_2_3__Unit_of_product;
												questionnaire.Goose_2_4__Per_No__Kg_bodyweight_min = row["Goose#2#4##Per#No##Kg#bodyweight_min"].ToString();
												questionnaire.Goose_2_5__Per_No__Kg_bodyweight_max = row["Goose#2#5##Per#No##Kg#bodyweight_max"].ToString();
												questionnaire.Goose_2_6__Duration_of_usage = row["Goose#2#6##Duration#of#usage"].ToString();


												questionnaire.Goose_3_1__Product_preparation__dilution__Product_amount = row["Goose#3#1##Product#preparation##dilution#_Product#amount"].ToString();
												questionnaire.Goose_3_2__Product_preparation_Unit_of_product = Goose_3_2__Product_preparation_Unit_of_product;
												questionnaire.Goose_3_3__Product_preparation_To_be_added_to__min_ = row["Goose#3#3##Product#preparation_To#be#added#to##min#"].ToString();
												questionnaire.Goose_3_4__Product_preparation_To_be_added_to__max_ = row["Goose#3#4##Product#preparation_To#be#added#to##max#"].ToString();
												questionnaire.Goose_3_5__Product_preparation_Unit_of_water_feed = row["Goose#3#5##Product#preparation_Unit#of#water#feed"].ToString();
												questionnaire.Goose_3_6__Duration_of_usage = row["Goose#3#6##Duration#of#usage"].ToString();



												questionnaire.Goose_4_1__Product_min = row["Goose#4#1##Product#min"].ToString();
												questionnaire.Goose_4_2__Product_max = row["Goose#4#2##Product#max"].ToString();
												questionnaire.Goose_4_3__Unit_of_product = Goose_4_3__Unit_of_product;
												questionnaire.Goose_4_4_Per_No__Kg_bodyweight_min = row["Goose#4#4#Per#No##Kg#bodyweight_min"].ToString();
												questionnaire.Goose_4_5__Per_No__Kg_bodyweight_max = row["Goose#4#5##Per#No##Kg#bodyweight_max"].ToString();
												questionnaire.Goose_4_6__Duration_of_usage = row["Goose#4#6##Duration#of#usage"].ToString();

												#endregion

											}
										}

									}
								}
							}
						}
					}
					foreach (Questionnaire q in questionnaires)
					{
						try
						{
							unitWork.Questionnaire.Update(q);
							unitWork.Commit();
						}
						catch { }


					}
				}

				if (filePath.Length > 0)
				{
					DataSet ds = new DataSet();

					string ConnectionString = "";
					if (filePath.EndsWith(".xls"))
					{
						ConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", filePath);
					}
					else if (filePath.EndsWith(".xlsx"))
					{
						ConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", filePath);
					}
					using (OleDbConnection conn = new System.Data.OleDb.OleDbConnection(ConnectionString))
					{
						conn.Open();
						using (DataTable dtExcelSchema = conn.GetSchema("Tables"))
						{
							string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();

							string query2 = "SELECT * FROM [" + sheetName + "$QK1:UD12000]";
							OleDbDataAdapter adapter2 = new OleDbDataAdapter(query2, conn);
							//DataSet ds = new DataSet();
							adapter2.Fill(ds, "Items");
							if (ds.Tables.Count > 0)
							{
								if (ds.Tables[0].Rows.Count > 0)
								{
									for (int i = 0; i < ds.Tables[0].Rows.Count; i++)//ds.Tables[0].Rows.Count
									{
										DataRow row = ds.Tables[0].Rows[i];
										foreach (var questionnaire in questionnaires)
										{
											if (questionnaire.keyupload == (i + 1).ToString())
											{

												#region R.Dog
												var Dog_1_2__Product_preparation_Unit_of_product = "";
												var Dog_2_3__Unit_of_product = "";
												var Dog_3_2__Product_preparation_Unit_of_product = "";
												var Dog_4_3__Unit_of_product = "";
												foreach (var item in lsunitvolumn)
												{
													string temp_unit = row["Dog#1#2##Product#preparation_Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)
													{
														Dog_1_2__Product_preparation_Unit_of_product = item;
													}

													temp_unit = row["Dog#2#3##Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)
													{
														Dog_2_3__Unit_of_product = item;
													}

													temp_unit = row["Dog#3#2##Product#preparation_Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)
													{
														Dog_3_2__Product_preparation_Unit_of_product = item;
													}

													temp_unit = row["Dog#4#3##Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)
													{
														Dog_4_3__Unit_of_product = item;
													}
												}

												questionnaire.Dog_1_1__Product_preparation__dilution__Product_amount = row["Dog#1#1##Product#preparation##dilution#_Product#amount"].ToString();
												questionnaire.Dog_1_2__Product_preparation_Unit_of_product = Dog_1_2__Product_preparation_Unit_of_product;
												questionnaire.Dog_1_3__Product_preparation_To_be_added_to__min_ = row["Dog#1#3##Product#preparation_To#be#added#to##min#"].ToString();
												questionnaire.Dog_1_4__Product_preparation_To_be_added_to__max_ = row["Dog#1#4##Product#preparation_To#be#added#to##max#"].ToString();
												questionnaire.Dog_1_5__Product_preparation_Unit_of_water_feed = row["Dog#1#5##Product#preparation_Unit#of#water#feed"].ToString();
												questionnaire.Dog_1_6__Duration_of_usage = row["Dog#1#6##Duration#of#usage"].ToString();


												questionnaire.Dog_2_1__Product_min = row["Dog#2#1##Product#min"].ToString();
												questionnaire.Dog_2_2__Product_max = row["Dog#2#2##Product#max"].ToString();
												questionnaire.Dog_2_3__Unit_of_product = Dog_2_3__Unit_of_product;
												questionnaire.Dog_2_4__Per_No__Kg_bodyweight_min = row["Dog#2#4##Per#No##Kg#bodyweight_min"].ToString();
												questionnaire.Dog_2_5__Per_No__Kg_bodyweight_max = row["Dog#2#5##Per#No##Kg#bodyweight_max"].ToString();
												questionnaire.Dog_2_6__Duration_of_usage = row["Dog#2#6##Duration#of#usage"].ToString();


												questionnaire.Dog_3_1__Product_preparation__dilution__Product_amount = row["Dog#3#1##Product#preparation##dilution#_Product#amount"].ToString();
												questionnaire.Dog_3_2__Product_preparation_Unit_of_product = Dog_3_2__Product_preparation_Unit_of_product;
												questionnaire.Dog_3_3__Product_preparation_To_be_added_to__min_ = row["Dog#3#3##Product#preparation_To#be#added#to##min#"].ToString();
												questionnaire.Dog_3_4__Product_preparation_To_be_added_to__max_ = row["Dog#3#4##Product#preparation_To#be#added#to##max#"].ToString();
												questionnaire.Dog_3_5__Product_preparation_Unit_of_water_feed = row["Dog#3#5##Product#preparation_Unit#of#water#feed"].ToString();
												questionnaire.Dog_3_6__Duration_of_usage = row["Dog#3#6##Duration#of#usage"].ToString();



												questionnaire.Dog_4_1__Product_min = row["Dog#4#1##Product#min"].ToString();
												questionnaire.Dog_4_2__Product_max = row["Dog#4#2##Product#max"].ToString();
												questionnaire.Dog_4_3__Unit_of_product = Dog_4_3__Unit_of_product;
												questionnaire.Dog_4_4_Per_No__Kg_bodyweight_min = row["Dog#4#4#Per#No##Kg#bodyweight_min"].ToString();
												questionnaire.Dog_4_5__Per_No__Kg_bodyweight_max = row["Dog#4#5##Per#No##Kg#bodyweight_max"].ToString();
												questionnaire.Dog_4_6__Duration_of_usage = row["Dog#4#6##Duration#of#usage"].ToString();

												#endregion

												#region S.Cat
												var Cat_1_2__Product_preparation_Unit_of_product = "";
												var Cat_2_3__Unit_of_product = "";
												var Cat_3_2__Product_preparation_Unit_of_product = "";
												var Cat_4_3__Unit_of_product = "";
												foreach (var item in lsunitvolumn)
												{
													string temp_unit = row["Cat#1#2##Product#preparation_Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)												
													{
														Cat_1_2__Product_preparation_Unit_of_product = item;
													}
													temp_unit = row["Cat#2#3##Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)													
													{
														Cat_2_3__Unit_of_product = item;
													}

													temp_unit = row["Cat#3#2##Product#preparation_Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)													
													{
														Cat_3_2__Product_preparation_Unit_of_product = item;
													}

													temp_unit = row["Cat#4#3##Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)													
													{
														Cat_4_3__Unit_of_product = item;
													}
												}

												questionnaire.Cat_1_1__Product_preparation__dilution__Product_amount = row["Cat#1#1##Product#preparation##dilution#_Product#amount"].ToString();
												questionnaire.Cat_1_2__Product_preparation_Unit_of_product = Cat_1_2__Product_preparation_Unit_of_product;
												questionnaire.Cat_1_3__Product_preparation_To_be_added_to__min_ = row["Cat#1#3##Product#preparation_To#be#added#to##min#"].ToString();
												questionnaire.Cat_1_4__Product_preparation_To_be_added_to__max_ = row["Cat#1#4##Product#preparation_To#be#added#to##max#"].ToString();
												questionnaire.Cat_1_5__Product_preparation_Unit_of_water_feed = row["Cat#1#5##Product#preparation_Unit#of#water#feed"].ToString();
												questionnaire.Cat_1_6__Duration_of_usage = row["Cat#1#6##Duration#of#usage"].ToString();


												questionnaire.Cat_2_1__Product_min = row["Cat#2#1##Product#min"].ToString();
												questionnaire.Cat_2_2__Product_max = row["Cat#2#2##Product#max"].ToString();
												questionnaire.Cat_2_3__Unit_of_product = Cat_2_3__Unit_of_product;
												questionnaire.Cat_2_4__Per_No__Kg_bodyweight_min = row["Cat#2#4##Per#No##Kg#bodyweight_min"].ToString();
												questionnaire.Cat_2_5__Per_No__Kg_bodyweight_max = row["Cat#2#5##Per#No##Kg#bodyweight_max"].ToString();
												questionnaire.Cat_2_6__Duration_of_usage = row["Cat#2#6##Duration#of#usage"].ToString();


												questionnaire.Cat_3_1__Product_preparation__dilution__Product_amount = row["Cat#3#1##Product#preparation##dilution#_Product#amount"].ToString();
												questionnaire.Cat_3_2__Product_preparation_Unit_of_product = Cat_3_2__Product_preparation_Unit_of_product;
												questionnaire.Cat_3_3__Product_preparation_To_be_added_to__min_ = row["Cat#3#3##Product#preparation_To#be#added#to##min#"].ToString();
												questionnaire.Cat_3_4__Product_preparation_To_be_added_to__max_ = row["Cat#3#4##Product#preparation_To#be#added#to##max#"].ToString();
												questionnaire.Cat_3_5__Product_preparation_Unit_of_water_feed = row["Cat#3#5##Product#preparation_Unit#of#water#feed"].ToString();
												questionnaire.Cat_3_6__Duration_of_usage = row["Cat#3#6##Duration#of#usage"].ToString();



												questionnaire.Cat_4_1__Product_min = row["Cat#4#1##Product#min"].ToString();
												questionnaire.Cat_4_2__Product_max = row["Cat#4#2##Product#max"].ToString();
												questionnaire.Cat_4_3__Unit_of_product = Cat_4_3__Unit_of_product;
												questionnaire.Cat_4_4_Per_No__Kg_bodyweight_min = row["Cat#4#4#Per#No##Kg#bodyweight_min"].ToString();
												questionnaire.Cat_4_5__Per_No__Kg_bodyweight_max = row["Cat#4#5##Per#No##Kg#bodyweight_max"].ToString();
												questionnaire.Cat_4_6__Duration_of_usage = row["Cat#4#6##Duration#of#usage"].ToString();

												#endregion

												#region T.Calf
												var Calf_1_2__Product_preparation_Unit_of_product = "";
												var Calf_2_3__Unit_of_product = "";
												var Calf_3_2__Product_preparation_Unit_of_product = "";
												var Calf_4_3__Unit_of_product = "";
												foreach (var item in lsunitvolumn)
												{
													string temp_unit = row["Calf#1#2##Product#preparation_Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)
													{
														Calf_1_2__Product_preparation_Unit_of_product = item;
													}
													temp_unit = row["Calf#2#3##Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)
													{
														Calf_2_3__Unit_of_product = item;
													}
													temp_unit = row["Calf#3#2##Product#preparation_Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)
													{
														Calf_3_2__Product_preparation_Unit_of_product = item;
													}

													temp_unit = row["Calf#4#3##Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)
													{
														Calf_4_3__Unit_of_product = item;
													}
												}

												questionnaire.Calf_1_1__Product_preparation__dilution__Product_amount = row["Calf#1#1##Product#preparation##dilution#_Product#amount"].ToString();
												questionnaire.Calf_1_2__Product_preparation_Unit_of_product = Calf_1_2__Product_preparation_Unit_of_product;
												questionnaire.Calf_1_3__Product_preparation_To_be_added_to__min_ = row["Calf#1#3##Product#preparation_To#be#added#to##min#"].ToString();
												questionnaire.Calf_1_4__Product_preparation_To_be_added_to__max_ = row["Calf#1#4##Product#preparation_To#be#added#to##max#"].ToString();
												questionnaire.Calf_1_5__Product_preparation_Unit_of_water_feed = row["Calf#1#5##Product#preparation_Unit#of#water#feed"].ToString();
												questionnaire.Calf_1_6__Duration_of_usage = row["Calf#1#6##Duration#of#usage"].ToString();


												questionnaire.Calf_2_1__Product_min = row["Calf#2#1##Product#min"].ToString();
												questionnaire.Calf_2_2__Product_max = row["Calf#2#2##Product#max"].ToString();
												questionnaire.Calf_2_3__Unit_of_product = Calf_2_3__Unit_of_product;
												questionnaire.Calf_2_4__Per_No__Kg_bodyweight_min = row["Calf#2#4##Per#No##Kg#bodyweight_min"].ToString();
												questionnaire.Calf_2_5__Per_No__Kg_bodyweight_max = row["Calf#2#5##Per#No##Kg#bodyweight_max"].ToString();
												questionnaire.Calf_2_6__Duration_of_usage = row["Calf#2#6##Duration#of#usage"].ToString();


												questionnaire.Calf_3_1__Product_preparation__dilution__Product_amount = row["Calf#3#1##Product#preparation##dilution#_Product#amount"].ToString();
												questionnaire.Calf_3_2__Product_preparation_Unit_of_product = Calf_3_2__Product_preparation_Unit_of_product;
												questionnaire.Calf_3_3__Product_preparation_To_be_added_to__min_ = row["Calf#3#3##Product#preparation_To#be#added#to##min#"].ToString();
												questionnaire.Calf_3_4__Product_preparation_To_be_added_to__max_ = row["Calf#3#4##Product#preparation_To#be#added#to##max#"].ToString();
												questionnaire.Calf_3_5__Product_preparation_Unit_of_water_feed = row["Calf#3#5##Product#preparation_Unit#of#water#feed"].ToString();
												questionnaire.Calf_3_6__Duration_of_usage = row["Calf#3#6##Duration#of#usage"].ToString();



												questionnaire.Calf_4_1__Product_min = row["Calf#4#1##Product#min"].ToString();
												questionnaire.Calf_4_2__Product_max = row["Calf#4#2##Product#max"].ToString();
												questionnaire.Calf_4_3__Unit_of_product = Calf_4_3__Unit_of_product;
												questionnaire.Calf_4_4_Per_No__Kg_bodyweight_min = row["Calf#4#4#Per#No##Kg#bodyweight_min"].ToString();
												questionnaire.Calf_4_5__Per_No__Kg_bodyweight_max = row["Calf#4#5##Per#No##Kg#bodyweight_max"].ToString();
												questionnaire.Calf_4_6__Duration_of_usage = row["Calf#4#6##Duration#of#usage"].ToString();

												#endregion

												#region R.Chick
												var Chick_1_2__Product_preparation_Unit_of_product = "";
												var Chick_2_3__Unit_of_product = "";
												var Chick_3_2__Product_preparation_Unit_of_product = "";
												var Chick_4_3__Unit_of_product = "";
												foreach (var item in lsunitvolumn)
												{
													string temp_unit = row["Chick#Duckling#1#2##Product#preparation_Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)
													{
														Chick_1_2__Product_preparation_Unit_of_product = item;
													}
													temp_unit = row["Chick#Duckling#2#3##Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)
													{
														Chick_2_3__Unit_of_product = item;
													}


													temp_unit = row["Chick#Duckling#3#2##Product#preparation_Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)
													{
														Chick_3_2__Product_preparation_Unit_of_product = item;
													}

													temp_unit = row["Chick#Duckling#4#3##Unit#of#product"].ToString().Trim();
													if (temp_unit == "UI") temp_unit = "IU";

													if (temp_unit == item)
													{
														Chick_4_3__Unit_of_product = item;
													}
												}
												questionnaire.Chick_1_1__Product_preparation__dilution__Product_amount = row["Chick#Duckling#1#1##Product#preparation##dilution#_Product#amoun"].ToString();
												questionnaire.Chick_1_2__Product_preparation_Unit_of_product = Chick_1_2__Product_preparation_Unit_of_product;
												questionnaire.Chick_1_3__Product_preparation_To_be_added_to__min_ = row["Chick#Duckling#1#3##Product#preparation_To#be#added#to##min#"].ToString();
												questionnaire.Chick_1_4__Product_preparation_To_be_added_to__max_ = row["Chick#Duckling#1#4##Product#preparation_To#be#added#to##max#"].ToString();
												questionnaire.Chick_1_5__Product_preparation_Unit_of_water_feed = row["Chick#Duckling#1#5##Product#preparation_Unit#of#water#feed"].ToString();
												questionnaire.Chick_1_6__Duration_of_usage = row["Chick#Duckling#1#6##Duration#of#usage"].ToString();


												questionnaire.Chick_2_1__Product_min = row["Chick#Duckling#2#1##Product#min"].ToString();
												questionnaire.Chick_2_2__Product_max = row["Chick#Duckling#2#2##Product#max"].ToString();
												questionnaire.Chick_2_3__Unit_of_product = Chick_2_3__Unit_of_product;
												questionnaire.Chick_2_4__Per_No__Kg_bodyweight_min = row["Chick#Duckling#2#4##Per#No##Kg#bodyweight_min"].ToString();
												questionnaire.Chick_2_5__Per_No__Kg_bodyweight_max = row["Chick#Duckling#2#5##Per#No##Kg#bodyweight_max"].ToString();
												questionnaire.Chick_2_6__Duration_of_usage = row["Chick#Duckling#2#6##Duration#of#usage"].ToString();


												questionnaire.Chick_3_1__Product_preparation__dilution__Product_amount = row["Chick#Duckling#3#1##Product#preparation##dilution#_Product#amoun"].ToString();
												questionnaire.Chick_3_2__Product_preparation_Unit_of_product = Chick_3_2__Product_preparation_Unit_of_product;
												questionnaire.Chick_3_3__Product_preparation_To_be_added_to__min_ = row["Chick#Duckling#3#3##Product#preparation_To#be#added#to##min#"].ToString();
												questionnaire.Chick_3_4__Product_preparation_To_be_added_to__max_ = row["Chick#Duckling#3#4##Product#preparation_To#be#added#to##max#"].ToString();
												questionnaire.Chick_3_5__Product_preparation_Unit_of_water_feed = row["Chick#Duckling#3#5##Product#preparation_Unit#of#water#feed"].ToString();
												questionnaire.Chick_3_6__Duration_of_usage = row["Chick#Duckling#3#6##Duration#of#usage"].ToString();



												questionnaire.Chick_4_1__Product_min = row["Chick#Duckling#4#1##Product#min"].ToString();
												questionnaire.Chick_4_2__Product_max = row["Chick#Duckling#4#2##Product#max"].ToString();
												questionnaire.Chick_4_3__Unit_of_product = Chick_4_3__Unit_of_product;
												questionnaire.Chick_4_4_Per_No__Kg_bodyweight_min = row["Chick#Duckling#4#4#Per#No##Kg#bodyweight_min"].ToString();
												questionnaire.Chick_4_5__Per_No__Kg_bodyweight_max = row["Chick#Duckling#4#5##Per#No##Kg#bodyweight_max"].ToString();
												questionnaire.Chick_4_6__Duration_of_usage = row["Chick#Duckling#4#6##Duration#of#usage"].ToString();

												#endregion
											}
										}

									}
								}
							}
						}
					}

				}

				foreach (Questionnaire q in questionnaires)
				{
					try {
						unitWork.Questionnaire.Update(q);
						unitWork.Commit();
					}
					catch { }
					
					
				}
				
			}
			catch (Exception ex) { throw ex; return ex.Message; }
			return "";
		}

	}
}