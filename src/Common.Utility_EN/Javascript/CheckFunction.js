/******************利用正则表达式，在字符串中，对特殊的字符： ' " < > & 进行编码*******************/
//参数：strSource ---- 需要替换的源字符串
//返回：编码过的字符串
function EncodeSpecialChar(strSource) {
    var stEncodeResult = strSource;

    //空字符串
    if (stEncodeResult == "") {
        return stEncodeResult;
    }

    //把字符串中的 "&" 字符替换成 "&amp;"
    //替换时，一定得先替换 "&" 字符，否则，会把 "<" 等编码中的 "&" 也进行替换，从而产生错误的结果
    var regExpInfo = /&/g; //利用正则表达式全局标识设置的 "&"，把该字符串中所有的 "&" 替换成 "&amp;"
    stEncodeResult = stEncodeResult.replace(regExpInfo, "＆");

    //把 ' 替换成 "‘" 
    regExpInfo = /'/g;
    stEncodeResult = stEncodeResult.replace(regExpInfo, "’");

    //把 " 替换成 "“" 
    regExpInfo = /"/g;
    stEncodeResult = stEncodeResult.replace(regExpInfo, "“");

    //把 < 替换成 "《" 
    regExpInfo = /</g;
    stEncodeResult = stEncodeResult.replace(regExpInfo, "《");

    //把 > 替换成 "》" 
    regExpInfo = />/g;
    stEncodeResult = stEncodeResult.replace(regExpInfo, "》");

    //把 % 替换成 "％" 
    regExpInfo = /%/g;
    stEncodeResult = stEncodeResult.replace(regExpInfo, "％");

    return stEncodeResult;
}

/*
*功能：检查字符串长度,超过最大长度返回false
*参数：strValue 字符串,maxLength 允许输入最大长度
*/
function CheckValueLength(strValue, maxLength) {
    return (strValue.length <= maxLength) ? true : false;
}

/*
*功能：检查字符串是否为空,为空则返回true
*参数：strValue 字符串
*/
function CheckEmpty(strValue) {
    return (!(strValue != null & strValue != "")) ? true : false;
}

/**
* 验证整数,包含正整数和负整数
*/
function CheckINTEGER(strValue) {
    var regTextInteger = /^(-|\+)?(\d)*$/;
    return regTextInteger.test(strValue);
}

/**
*2 positive integer检查是否为正整数 (  /^[1-9]+[0-9]*]*$/
*/
function CheckPositiveInt(strValue) {
    var regExpInfo = /^[1-9]+[0-9]*]*$/;
    return regExpInfo.test(strValue);
}

/**
check number input control
xxxxx3 检查是否输入的是数字,并保留 正确输入的数字
*参数: objInput, 控件对象
*/
function CheckNumInput(objInput) {
    var i = 0, returnVal = '';
    var strInput = objInput.value;
    for (i = 0; i < strInput.length; i++) {
        if (returnVal == '') {
            if (CheckPositiveInt(strInput.charAt(i))) //显示的第1个数字为正整数
                returnVal = returnVal.concat(strInput.charAt(i));
        }
        else {
            if (!CheckNUMBER(strInput.charAt(i))) break;
            returnVal = returnVal.concat(strInput.charAt(i));
        }
    }
    if (strInput != '' & returnVal == '') {
        alert('内容必须为英文或半角数字!');
        objInput.focus();
    }
    objInput.value = returnVal;
}

/**
*验证钱数,带单位
*/
function CheckMoney(strValue, strUnit) {
    var testMoney = eval("/^\\d+(\\.\\d{0," + (strUnit.length - 1) + "})?$/");
    return testMoney.test(strValue);
}

/**
* 验证浮点数
*/
function CheckFLOAT(strValue) {
    var regTextFloat = /^(-)?(\d)*(\.)?(\d)*$/;
    return regTextFloat.test(strValue);
}

/**
* 验证数字
*/
function CheckNUMBER(strValue) {
    var regTextNumber = /^(\d)*$/;
    return regTextNumber.test(strValue);
}

/**
* 验证英文字母，不区分大小写
*/
function CheckTextForENGLISH(strValue) {
    var regTextEnglish = /^[a-zA-Z]*$/;
    return regTextEnglish.test(strValue);
}

/**
* 验证英文字母和数字，不区分大小写
*/
function CheckTextForENGLISHNUMBER(strValue) {
    var regTextEnglishNumber = /^[a-zA-Z0-9]*$/;
    return regTextEnglishNumber.test(strValue);
}

/**
* 验证电话号码
*/
function CheckPHONE(strValue) {
    var regExpInfo = /^(\(\d{3,4}\)|\d{3,4}-|\s)?\d{7,14}$/;
    return regExpInfo.test(strValue);
}

/**
* 验证时间  (XXX问题 只是时间格式)
*/
function CheckTextDataForTIME(strValue) {
    var regTextTime = /^(\d+):(\d{1,2}):(\d{1,2})$/;
    return regTextTime.test(strValue);
}

/**
* 验证EMail
*/
function CheckEMAIL(strValue) {
    var regTextEmail = /^[\w-]+@[\w-]+(\.(\w)+)*(\.(\w){2,3})$/;
    return regTextEmail.test(strValue);
}

/**
* 验证URL
*/
function CheckURL(strValue) {
    var regTextUrl = /^(file|http|https|ftp|mms|telnet|news|wais|mailto):\/\/(.+)$/;
    return regTextUrl.test(strValue);
}

/**
*验证身份证
*/
function CheckIdCard(strValue) {
    var regExpInfo = /^(\w{15}|\w{18})$/;
    return regExpInfo.test(strValue);
}

/************** 比较两个时间大小,开始时间比结束时间早则返回true **************/
//参数: obj_dateBegain 开始时间字符串,obj_dateEnd 结束时间字符串 (日期格式:yyyy-mm-dd hh:mi:ss,eg 2008-08-08)
function CheckComDate(obj_dateBegain, obj_dateEnd) {
    var dates, datee;
    dates = new Date(obj_dateBegain.substr(0, 4), obj_dateBegain.substr(5, 2), obj_dateBegain.substr(8, 2));
    datee = new Date(obj_dateEnd.substr(0, 4), obj_dateEnd.substr(5, 2), obj_dateEnd.substr(8, 2));
    if (dates <= datee) {
        if (dates == datee) {
            var dates1, datee1;
            dates1 = new Date(obj_dateBegain.substr(0, 4), obj_dateBegain.substr(5, 2)
				, obj_dateBegain.substr(8, 2), obj_dateBegain.substr(11, 2), obj_dateBegain.substr(14, 2)
				, obj_dateBegain.substr(17, 2));
            datee1 = new Date(obj_dateEnd.substr(0, 4), obj_dateEnd.substr(5, 2), obj_dateEnd.substr(8, 2)
			, obj_dateEnd.substr(11, 2), obj_dateEnd.substr(14, 2), obj_dateEnd.substr(17, 2));
            if (dates1 <= datee1)
                return true;
            else
                return false;
        }
        else {
            return true;
        }
    }
    else {
        return false;
    }
}

/*****************检查是否存在 “< > " '& \ / ; |”等特殊字符*****************/
//返回：true ---- 包含此不合法字符  false ---- 不包含
function CheckSpecialChar(strSource) {
    var intIndex = -1; //没找到此字符，返回-1

    var regExpInfo = /&/;
    intIndex = strSource.search(regExpInfo);

    if (intIndex == -1) {
        regExpInfo = /</;
        intIndex = strSource.search(regExpInfo);
    }

    if (intIndex == -1) {
        regExpInfo = />/;
        intIndex = strSource.search(regExpInfo);
    }

    if (intIndex == -1) {
        regExpInfo = /"/;
        intIndex = strSource.search(regExpInfo);
    }

    if (intIndex == -1) {
        regExpInfo = /'/;
        intIndex = strSource.search(regExpInfo);
    }

    if (intIndex == -1) {
        regExpInfo = /;/;
        intIndex = strSource.search(regExpInfo);
    }

    if (intIndex == -1) {
        regExpInfo = /\|/;
        intIndex = strSource.search(regExpInfo);
    }

    if (intIndex == -1) {
        regExpInfo = /\//;
        intIndex = strSource.search(regExpInfo);
    }

    if (intIndex == -1) {
        regExpInfo = /\\/;
        intIndex = strSource.search(regExpInfo);
    }

    if (intIndex == -1) {
        return false;
    }
    else {
        return true;
    }
}

/**********************检查固定小数位的浮点数字********************************/
function CheckFixFloat(objName) {
    var regExpInfo = /^-?\d+$/;
    if (objName.value.search(regExpInfo) >= 0) {
        return true;
    }
    else {
        var regExpInfo1 = /^\d+.[0-9]{1,2}$/;
        if (objName.value.search(regExpInfo1) >= 0) {
            return true;
        }
        else {
            if (Trim(objName.value) != "") {
                alert("输入内容必须为整数或小数！小数点后最多2位数");
                objName.value = "0.00";
                objName.focus();
            }
            return false;
        }
    }
}

/******************** 检查字符串 字母，数字或下划线字符 ***********************/
function CheckName(objName) {
    var regExpInfo = /\W/;
    if (objName.value.replace(".", "").search(regExpInfo) >= 0) {
        alert("输入内容必须为字母，数字或下划线!");
        objName.value = "";
        return false;
    }
    else {
        return true;
    }
}


/************************ 只允许输入数字 ************************/
function NumOnly() {
    var i = window.event.keyCode;
    if ((i <= 57 && i >= 45) || (i >= 96 && i <= 105) || (i == 8) || (i == 9) || (i == 37) || (i == 39) || (i == 46) || (i == 17)) {
        return true;
    }
    else {
        event.returnValue = false;
        return false;
    }
}

/************************ 只允许输入数字和小数点,负号 ************************/
function FloatOnly() {
    var i = window.event.keyCode;
    if ((i <= 57 && i >= 45) || (i >= 96 && i <= 105) || (i == 8) || (i == 9) || (i == 37) || (i == 39) || (i == 46) || (i == 17) || (i == 189) || (i == 190)) {
        return true;
    }
    else {
        event.returnValue = false;
        return false;
    }
}

/************************ 检查TextArea文本输入的有效值 ************************/
function checkLength(which, maxlength) {
    if (which.value.length > maxlength) {
        alert('超过最大长度' + maxlength + '，系统自动截取有效值！')
        which.value = which.value.substring(0, maxlength);
    }
}

/************************ 获得OnChange,OnBlur时检查整数的JS ************************/
function EventCheckInt(objControl, chineseName) {
    if (!CheckINTEGER(RemoveOldValue(objControl, oldValue))) {
        objControl.value = oldValue;
        alert(chineseName + "内容不是有效数字!");
        objControl.focus();
    }
    else {
        oldValue = objControl.value;
    }
}

/************************ 获得OnChange,OnBlur时检查浮点数的JS ************************/
function EventCheckFloat(objControl, chineseName) {
    if (!CheckFLOAT(RemoveOldValue(objControl, oldValue))) {
        objControl.value = oldValue;
        alert(chineseName + "内容不是有效数字!");
        objControl.focus();
    }
    else {
        oldValue = objControl.value;
    }
}

/************************ 获得OnChange,OnBlur时检查特殊字符输入的JS ************************/
function EventCheckSpecialChar(objControl, chineseName) {
    if (!CheckTextForNORMAL(RemoveOldValue(objControl, oldValue))) {
        objControl.value = oldValue;
        alert(chineseName + "内容不能包含 * \"  < > / ) 等特殊字符!");
        objControl.focus();
    }
    else {
        oldValue = objControl.value;
    }
}

/************************ 获得OnChange,OnBlur时检查特殊字符输入的JS ************************/
function EventCheckMaxLength(objControl, chineseName, intMaxLength) {
    if (!CheckValueLength(objControl.value, intMaxLength)) {
        objControl.value = objControl.value.substring(0, intMaxLength);
        alert(chineseName + "超过最大长度" + intMaxLength + ',系统自动截取有效值!');
        objControl.focus();
    }
    else {
        oldValue = objControl.value;
    }
}

/************************ 验证普通字串 ************************/
//只要字串中不包含特殊字符星号、大于号、小于号、单引号、左括号、右括号、空格等即可
function CheckTextForNORMAL(strValue) {
    var regTextChar = /([\*\"\'<>\/\(\&\)\\\卐\卍\ ])+/;
    return !regTextChar.test(strValue);
} 