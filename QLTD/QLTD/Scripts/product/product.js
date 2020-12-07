
function Product(lsAntimi, lsOrtherAB, lsAnimalInfor) {
    this.lsAntimi = lsAntimi;
    this.lsOrtherAB = lsOrtherAB;
    this.lsAnimalInfor = lsAnimalInfor;

    this.AddAntimi = function (AntimicroBial) {
        lsAntimi.push(AntimicroBial);
        console.log(lsAntimi);
    }

    this.AddOrtherAB = function (OrtherAB) {
        lsOrtherAB.push(OrtherAB);
    }

    this.AddAnimal = function (AnimalInfor) {
        lsAnimalInfor.push(AnimalInfor);
    }

    this.UpdateAntimi = function (AntimicroBial) {
        var oldantimi = lsAntimi.find(x => x.Id == AntimicroBial.Id);
        oldantimi.Antimi = AntimicroBial.Antimi;
        oldantimi.Strength = AntimicroBial.Strength;
        oldantimi.Units = AntimicroBial.Units;
        oldantimi.PerAmountOfAnti = AntimicroBial.PerAmountOfAnti;
        oldantimi.UnitsOfPerAmountAnti = AntimicroBial.UnitsOfPerAmountAnti;
        oldantimi.PerAmountOfProduct = AntimicroBial.PerAmountOfProduct;
        oldantimi.UnitsOfPerAmountProduct = AntimicroBial.UnitsOfPerAmountProduct;
        oldantimi.Note = AntimicroBial.Note;
    }

    this.UpdateOrtherAB = function (OrtherAB) {
        var oldantimi = lsOrtherAB.find(x => x.Id == OrtherAB.Id);
        oldantimi.Name = OrtherAB.Name;
        oldantimi.Strength = OrtherAB.Strength;
        oldantimi.Units = OrtherAB.Units;
        oldantimi.PerAmountOfAnti = OrtherAB.PerAmountOfAnti;
        oldantimi.UnitsOfPerAmountAnti = OrtherAB.UnitsOfPerAmountAnti;
        oldantimi.PerAmountOfProduct = OrtherAB.PerAmountOfProduct;
        oldantimi.UnitsOfPerAmountProduct = OrtherAB.UnitsOfPerAmountProduct;
        oldantimi.Note = OrtherAB.Note;
    }

    this.UpdateAnimal = function (AnimalInfor) {
        var oldanimal = lsAnimalInfor.find(x => x.Id == AnimalInfor.Id);
        oldanimal.Animal = AnimalInfor.Animal;
        oldanimal.PB_Product_preparation__dilution__Product_amount = AnimalInfor.PB_Product_preparation__dilution__Product_amount;
        oldanimal.PB_Product_preparation_Unit_of_product = AnimalInfor.PB_Product_preparation_Unit_of_product;
        oldanimal.PB_Product_preparation_To_be_added_to__min_ = AnimalInfor.PB_Product_preparation_To_be_added_to__min_;
        oldanimal.PB_Product_preparation_To_be_added_to__max_ = AnimalInfor.PB_Product_preparation_To_be_added_to__max_;
        oldanimal.PB_Product_preparation_Unit_of_water_feed = AnimalInfor.PB_Product_preparation_Unit_of_water_feed;
        oldanimal.PB_Duration_of_usages = AnimalInfor.PB_Duration_of_usages;
        oldanimal.PB_Product_min = AnimalInfor.PB_Product_min;
        oldanimal.PB_Product_max = AnimalInfor.PB_Product_max;
        oldanimal.PB_Unit_of_product = AnimalInfor.PB_Unit_of_product;
        oldanimal.PB_Per_No__Kg_bodyweight_min = AnimalInfor.PB_Per_No__Kg_bodyweight_min;
        oldanimal.PB_Per_No__Kg_bodyweight_max = AnimalInfor.PB_Per_No__Kg_bodyweight_max;
        oldanimal.PB_Duration_of_usage = AnimalInfor.PB_Duration_of_usage;
        oldanimal.TB_Product_preparation__dilution__Product_amount = AnimalInfor.TB_Product_preparation__dilution__Product_amount;
        oldanimal.TB_Product_preparation_Unit_of_product = AnimalInfor.TB_Product_preparation_Unit_of_product;
        oldanimal.TB_Product_preparation_To_be_added_to__min_ = AnimalInfor.TB_Product_preparation_To_be_added_to__min_;
        oldanimal.TB_Product_preparation_To_be_added_to__max_ = AnimalInfor.TB_Product_preparation_To_be_added_to__max_;
        oldanimal.TB_Product_preparation_Unit_of_water_feed = AnimalInfor.TB_Product_preparation_Unit_of_water_feed;
        oldanimal.TB_Product_preparation_Unit_of_water_feed = AnimalInfor.TB_Product_preparation_Unit_of_water_feed;
        oldanimal.TB_Duration_of_usage_MM = AnimalInfor.TB_Duration_of_usage_MM;
        oldanimal.TB_Product_min = AnimalInfor.TB_Product_min;
        oldanimal.TB_Product_max = AnimalInfor.TB_Product_max;
        oldanimal.TB_Unit_of_product = AnimalInfor.TB_Unit_of_product;
        oldanimal.TB_Per_No__Kg_bodyweight_min = AnimalInfor.TB_Per_No__Kg_bodyweight_min;
        oldanimal.TB_Per_No__Kg_bodyweight_max = AnimalInfor.TB_Per_No__Kg_bodyweight_max;
        oldanimal.TB_Duration_of_usage = AnimalInfor.TB_Duration_of_usage;
    }

    this.RemoveAntimi = function (Id) {
        var index = lsAntimi.map(x => {
            return x.Id;
        }).indexOf(Id);
        lsAntimi.splice(index, 1);
    }

    this.RemoveOrtherAB = function (Id) {
        var index = lsOrtherAB.map(x => {
            return x.Id;
        }).indexOf(Id);
        lsOrtherAB.splice(index, 1);
    }

    this.RemoveAnimal = function (Id) {
        var index = lsAnimalInfor.map(x => {
            return x.Id;
        }).indexOf(Id);
        lsAnimalInfor.splice(index, 1);
    }

    this.GetAntimiById = function (Id) {
        var oldantimi = lsAntimi.find(x => x.Id == Id);
        return oldantimi;
    }

    this.GetOrtherById = function (Id) {
        var oldantimi = lsOrtherAB.find(x => x.Id == Id);
        return oldantimi;
    }

    this.GetAnimalById = function (Id) {
        var oldanimal = lsAnimalInfor.find(x => x.Id == Id);
        return oldanimal;
    }

    this.CheckAntimiExist = function (Id,CurrentId) {
        var check = lsAntimi.find(x => x.Antimi.Id == Id);
        if (CurrentId != null && CurrentId != "") {
            if (check != null) {
                if (check.Id != CurrentId) {
                    return false;
                }
                return true
            }
        }
        if (check != null)
            return false;
        return true
    }

    this.CheckAnnimalExist = function (Id, CurrentId) {
        var check = lsAnimalInfor.find(x => x.Animal.Id == Id);
        if (CurrentId != null && CurrentId != "") {
            if (check != null) {
                if (check.Id != CurrentId) {
                    return false;
                }
                return true
            }
        }
        if (check != null)
            return false;
        return true
    }
}

function GetHtmlAntimi(newanticrobial) {
    var unit1 = ConvertUnitToString(newanticrobial.Units.toString());
    var unit2 = ConvertUnitToString(newanticrobial.UnitsOfPerAmountAnti.toString());
    var unit3 = ConvertUnitToString(newanticrobial.UnitsOfPerAmountProduct.toString());
    var html = '';
    html += '<tr id="tran_' + newanticrobial.Id + '">';
    html += '<td>' + newanticrobial.Antimi.Name + '</td>';
    html += '<td>' + newanticrobial.Strength + ' ' + unit1+ '</td>';
    html += '<td>' + newanticrobial.PerAmountOfAnti + ' ' + unit2  + '</td>';
    html += '<td>' + newanticrobial.PerAmountOfProduct + ' ' + unit3 + '</td>';
    html += '<td><button type="button" class="btn btn-sm btn-outline-success" onclick="EditAntimi(' + newanticrobial.Id + ')"><i class="fas fa-pencil-alt"></i></button>  </td>  ';
    html += '  <td>  <button type="button" class="btn btn-sm btn-outline-danger" onclick="DeleteAntimi(' + newanticrobial.Id + ')"><i class="fa fa-trash" aria-hidden="true"></i></button></td>';
    html += '</tr>'
    return html;
}
function GetHtmlOrther(neworther) {
    var unit1 = ConvertUnitToString(newanticrobial.Units.toString());
    var unit2 = ConvertUnitToString(newanticrobial.UnitsOfPerAmountAnti.toString());
    var unit3 = ConvertUnitToString(newanticrobial.UnitsOfPerAmountProduct.toString());
    var html = '';
    html += '<tr id="tror_' + neworther.Id + '">';
    html += '<td>' + neworther.Name + '</td>';
    html += '<td>' + neworther.Strength + ' ' + unit1 + '</td>';
    html += '<td>' + neworther.PerAmountOfAnti + ' ' + unit2 + '</td>';
    html += '<td>' + neworther.PerAmountOfProduct + ' ' + unit3 + '</td>';
    html += '<td><button type="button" class="btn btn-sm btn-outline-success" onclick="EditOrther(' + neworther.Id + ')"><i class="fas fa-pencil-alt"></i></button>  </td> ';
    html += ' <td> <button type="button" class="btn btn-sm btn-outline-danger" onclick="DeleteOrther(' + neworther.Id + ')"><i class="fa fa-trash" aria-hidden="true"></i></button></td>';
    html += '</tr>'
    return html;
}
function GetHtmlAnimal(newanimal) {
    var unit1 = ConvertUnitToString(newanimal.PB_Product_preparation_Unit_of_product.toString());
    var unit2 = ConvertUnitToString(newanimal.TB_Product_preparation_Unit_of_product.toString());
    var html = '';
    html += '<tr id="trni_' + newanimal.Id + '">';
    html += '<td>' + newanimal.Animal.Name + '</td>';
    html += '<td>' + newanimal.PB_Product_preparation__dilution__Product_amount + ' ' + unit1  + '</td>';
    html += '<td>' + newanimal.PB_Product_preparation_To_be_added_to__min_ + ' - ' + newanimal.PB_Product_preparation_To_be_added_to__max_ + ' ' + newanimal.PB_Product_preparation_Unit_of_water_feed  + '</td>';
    html += '<td>' + newanimal.PB_Per_No__Kg_bodyweight_min + ' - ' + newanimal.PB_Per_No__Kg_bodyweight_max + ' kg </td>';
    html += '<td>' + newanimal.TB_Product_preparation__dilution__Product_amount + ' ' + unit2 + '</td>';
    html += '<td>' + newanimal.TB_Product_preparation_To_be_added_to__min_ + ' - ' + newanimal.TB_Product_preparation_To_be_added_to__max_ + ' ' + newanimal.TB_Product_preparation_Unit_of_water_feed  + '</td>';
    html += '<td>' + newanimal.TB_Per_No__Kg_bodyweight_min + ' - ' + newanimal.TB_Per_No__Kg_bodyweight_max + ' kg </td>';
    html += '<td><button type="button" class="btn btn-sm btn-outline-success" onclick="EditAnimal(' + newanimal.Id + ')"><i class="fas fa-pencil-alt"></i></button>   ';
    html += '  <button type="button" class="btn btn-sm btn-outline-danger" onclick="DeleteAnimal(' + newanimal.Id + ')"><i class="fa fa-trash" aria-hidden="true"></i></button></td>';
    html += '</tr>'
    return html;
}

function ConvertUnitToString(unit) {
    var unitstring = '';
    switch (unit) {
        case "0":
            unitstring = "lít";
            break;
        case "1":
            unitstring = "kg";
            break;
        case "2":
            unitstring = "mg";
            break;
        case "3":
            unitstring = "g";
            break;
        case "4":
            unitstring = "IU";
            break;
        case "5":
            unitstring = "ml";
            break;
        case "6":
            unitstring = "-";
            break;
    }
    return unitstring;
}


function ProductInfor(Id, CollectedDate, ProductOrigin, ProductCode, ProductName, Company, TypeOfProduct, Other_Subtance_In_Product,
    Volume_Of_product, Unit_Of_Volume_Of_Product, Other_Volume_Of_Product, IsDope, IsAntimicrobial ,
    Source_of_information, Picture_of_product, Person_in_charge, Note) {
    this.Id = Id;
    this.CollectedDate = CollectedDate;
    this.ProductOrigin = ProductOrigin;
    this.ProductCode = ProductCode;
    this.ProductName = ProductName;
    this.Company = Company;
    this.TypeOfProduct = TypeOfProduct;
    this.Other_Subtance_In_Product = Other_Subtance_In_Product;
    this.Volume_Of_product = Volume_Of_product;
    this.Unit_Of_Volume_Of_Product = Unit_Of_Volume_Of_Product;
    this.Other_Volume_Of_Product = Other_Volume_Of_Product;
    this.IsAntimicrobial = IsAntimicrobial;
    this.IsDope = IsDope;
    this.Source_of_information = Source_of_information;
    this.Picture_of_product = Picture_of_product;
    this.Person_in_charge = Person_in_charge;
    this.Note = Note;
}
function AntimicroBial(Id, Antimi, Strength, Units, PerAmountOfAnti, UnitsOfPerAmountAnti, PerAmountOfProduct, UnitsOfPerAmountProduct, Note) {
    this.Id = Id;
    this.Antimi = Antimi;
    this.Strength = Strength;
    this.Units = Units;
    this.PerAmountOfAnti = PerAmountOfAnti;
    this.UnitsOfPerAmountAnti = UnitsOfPerAmountAnti;
    this.PerAmountOfProduct = PerAmountOfProduct;
    this.UnitsOfPerAmountProduct = UnitsOfPerAmountProduct;
    this.Note = Note;
}
function OrtherAB(Id, Name, Strength, Units, PerAmountOfAnti, UnitsOfPerAmountAnti, PerAmountOfProduct, UnitsOfPerAmountProduct, Note) {
    this.Id = Id;
    this.Name = Name;
    this.Strength = Strength;
    this.Units = Units;
    this.PerAmountOfAnti = PerAmountOfAnti;
    this.UnitsOfPerAmountAnti = UnitsOfPerAmountAnti;
    this.PerAmountOfProduct = PerAmountOfProduct;
    this.UnitsOfPerAmountProduct = UnitsOfPerAmountProduct;
    this.Note = Note;
}
function AnimalInfor(Id, Animal,
    PB_Product_preparation__dilution__Product_amount,
    PB_Product_preparation_Unit_of_product,
    PB_Product_preparation_To_be_added_to__min_,
    PB_Product_preparation_To_be_added_to__max_,
    PB_Product_preparation_Unit_of_water_feed,
    PB_Duration_of_usages,
    PB_Product_min,
    PB_Product_max,
    PB_Unit_of_product,
    PB_Per_No__Kg_bodyweight_min,
    PB_Per_No__Kg_bodyweight_max,
    PB_Duration_of_usage,
    TB_Product_preparation__dilution__Product_amount,
    TB_Product_preparation_Unit_of_product,
    TB_Product_preparation_To_be_added_to__min_,
    TB_Product_preparation_To_be_added_to__max_,
    TB_Product_preparation_Unit_of_water_feed,
    TB_Duration_of_usage_MM,
    TB_Product_min,
    TB_Product_max,
    TB_Unit_of_product,
    TB_Per_No__Kg_bodyweight_min,
    TB_Per_No__Kg_bodyweight_max,
    TB_Duration_of_usage) {

    this.Id = Id;
    this.Animal = Animal;
    this.PB_Product_preparation__dilution__Product_amount = PB_Product_preparation__dilution__Product_amount;
    this.PB_Product_preparation_Unit_of_product = PB_Product_preparation_Unit_of_product;
    this.PB_Product_preparation_To_be_added_to__min_ = PB_Product_preparation_To_be_added_to__min_;
    this.PB_Product_preparation_To_be_added_to__max_ = PB_Product_preparation_To_be_added_to__max_;
    this.PB_Product_preparation_Unit_of_water_feed = PB_Product_preparation_Unit_of_water_feed;
    this.PB_Duration_of_usages = PB_Duration_of_usages;
    this.PB_Product_min = PB_Product_min;
    this.PB_Product_max = PB_Product_max;
    this.PB_Unit_of_product = PB_Unit_of_product;
    this.PB_Per_No__Kg_bodyweight_min = PB_Per_No__Kg_bodyweight_min;
    this.PB_Per_No__Kg_bodyweight_max = PB_Per_No__Kg_bodyweight_max;
    this.PB_Duration_of_usage = PB_Duration_of_usage;
    this.TB_Product_preparation__dilution__Product_amount = TB_Product_preparation__dilution__Product_amount;
    this.TB_Product_preparation_Unit_of_product = TB_Product_preparation_Unit_of_product;
    this.TB_Product_preparation_To_be_added_to__min_ = TB_Product_preparation_To_be_added_to__min_;
    this.TB_Product_preparation_To_be_added_to__max_ = TB_Product_preparation_To_be_added_to__max_;
    this.TB_Product_preparation_Unit_of_water_feed = TB_Product_preparation_Unit_of_water_feed;
    this.TB_Duration_of_usage_MM = TB_Duration_of_usage_MM;
    this.TB_Product_min = TB_Product_min;
    this.TB_Product_max = TB_Product_max;
    this.TB_Unit_of_product = TB_Unit_of_product;
    this.TB_Per_No__Kg_bodyweight_min = TB_Per_No__Kg_bodyweight_min;
    this.TB_Per_No__Kg_bodyweight_max = TB_Per_No__Kg_bodyweight_max;
    this.TB_Duration_of_usage = TB_Duration_of_usage;
}
function Antimi(Id, Name) {
    this.Id = Id;
    this.Name = Name;
}
function Animal(Id, Name) {
    this.Id = Id;
    this.Name = Name;
}