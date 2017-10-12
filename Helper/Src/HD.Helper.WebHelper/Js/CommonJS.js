
/************************ 用户在button以外的页面元素中按回车转换为按tab键************************/
function OnKeyDownDefault()  
{  
    if(window.event.keyCode == 13 && window.event.ctrlKey == false && window.event.altKey == false) 
    { 
        if (window.event.srcElement.type !='button' && window.event.srcElement.type !='file' && window.event.srcElement.type !='submit') 
        { 
            window.event.keyCode = 9;
        }
        else
        { 
            return true;
        } 
    } 
}

/************************ 在隐藏控件中保存Select选中的Value ************************/
function SetHiddenValue(obj , hdnObj)
{
    hdnObj.value = obj.options[obj.selectedIndex].value;
}

/************************ 在隐藏控件中保存Select选中的Text ************************/
function SetHiddenText(obj, hdnObj) {
    hdnObj.value = obj.options[obj.selectedIndex].text;
}

/************************ 在隐藏控件中保存Radio选中的Text,RadioList中每个Radio均调用此方法 ************************/
function SetRadioHiddenValue( checkedText, hdnObj)
{
    hdnObj.value = checkedText ;
}

/************************ 在隐藏控件中保存Select选中的Text ************************/
var oldValue = '';
function GetFocusValue(objControl)
{
    oldValue = objControl.value;
}

/************************ 去掉原值检查新输入值的JS ************************/
function RemoveOldValue( objControl, checkValue)
{
    var newValue = '';
    if( objControl.value.search (checkValue) == 0)
    {
        //当在原值基础上输入时,从开始处去掉原值部分,仅检查新输入内容
        newValue = objControl.value.substring(checkValue.length);
    }
    else
    {
        newValue = objControl.value;
    }
    return newValue;
}

/************************ 生成检查输入值是否重复的脚本函数 ************************/
function CheckRepate(infields,invals,inQuotes,strBackId) 
{ 
    var feildName = 'feildName=';
    var inputVal = 'values='; 
    var haveQuotes = 'quotes=';
    var infieldsAry = infields.split(',');
    var invalsAry = invals.split(',');
    var inQuotesAry = inQuotes.split(',');
    for(var i=0; i<infieldsAry.length; i++ )
    { 
        feildName = feildName+ infieldsAry[i] +','; 
        inputVal = inputVal  + escape(document.getElementById(invalsAry[i]).value)+ ','; 
        haveQuotes = haveQuotes+ inQuotesAry[i] +','; 
    } 
    feildName=feildName.substr(0, feildName.length-1); 
    inputVal=inputVal.substr(0, inputVal.length-1); 
    haveQuotes=haveQuotes.substr(0, haveQuotes.length-1); 
    var strData = feildName+'&'+inputVal+'&'+haveQuotes;
    var cbo = new CallBackObject(strBackId);
    cbo.DoCallBack('',strData );
} 

/***************************从Cookie中取值****************************************/
function Get_Cookie(check_name) {
    var a_all_cookies = document.cookie.split(';');
    var a_temp_cookie = '';
    var cookie_name = '';
    var cookie_value = '';
    var b_cookie_found = false;
    var i = '';

    for (i = 0; i < a_all_cookies.length; i++) {
        a_temp_cookie = a_all_cookies[i].split('=');
        cookie_name = a_temp_cookie[0].replace(/^\s+|\s+$/g, '');
        if (cookie_name == check_name) {
            b_cookie_found = true;
            if (a_temp_cookie.length > 1) {
                cookie_value = unescape(a_temp_cookie[1].replace(/^\s+|\s+$/g, ''));
            }
            return cookie_value;
            break;
        }
        a_temp_cookie = null;
        cookie_name = '';
    }
    if (!b_cookie_found) {
        return null;
    }
}

/******************保存值到Cookie中**************************/
function Set_Cookie(name, value, expires, path, domain, secure) {
    var today = new Date();
    today.setTime(today.getTime());
    if (expires) {
        expires = expires * 1000 * 60 * 60 * 24;
    }
    var expires_date = new Date(today.getTime() + (expires));
    document.cookie = name + "=" + escape(value) +
		((expires) ? ";expires=" + expires_date.toGMTString() : "") + //expires.toGMTString()
		((path) ? ";path=" + path : "") +
		((domain) ? ";domain=" + domain : "") +
		((secure) ? ";secure" : "");
}

/********************删除Cookie*******************************/
function Delete_Cookie(name, path, domain) {
    if (Get_Cookie(name)) document.cookie = name + "=" +
			((path) ? ";path=" + path : "") +
			((domain) ? ";domain=" + domain : "") +
			";expires=Thu, 01-Jan-1970 00:00:01 GMT";
}

//功能：去掉字符串两边空格
//返回：true ---- 包含此不合法字符  false ---- 不包含
function TrimString(str) {
    var i, j;
    if (str == "") return "";
    for (i = 0; i < str.length; i++)
        if (str.charAt(i) != ' ') break;
    if (i >= str.length) return "";

    for (j = str.length - 1; j >= 0; j--)
        if (str.charAt(j) != ' ') break;

    return str.substring(i, j + 1);
}

//--除去前空白符
function Ltrim(str) {
    return str.replace(/^\s+/, "");
}

//--除去后空白符
function Rtrim(str) {
    return str.replace(/\s+$/, "");
}

//--除去前后空白符
function Trim(str) {
    return Ltrim(Rtrim(str));
}