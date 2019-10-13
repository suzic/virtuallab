
var editor;
var outer;

$(document).ready(function () {
    editor = CodeMirror.fromTextArea(document.getElementById('code_text'),
        {
            mode: 'text/x-c++src',
            lineNumbers: true,
            theme: 'mdn-like',
            matchBrackets: true,
            identUnit: 4,
            smartIdent: true,
            indentWithTabs: true
        });
    editor.setSize('100%', '100%');
    outer = CodeMirror.fromTextArea(document.getElementById('debug_text'),
        {
            mode: 'textile',
            theme: 'zenburn',
            identUnit: 4,
            readOnly: true
        });
    outer.setSize('100%', '100%');
});

function loadCodeFile(input) {
    //支持chrome IE10  
    if (window.FileReader) {
        var file = input.files[0];
        filename = file.name.split(".")[0];
        var reader = new FileReader();
        reader.onload = function () {
            editor.setValue(this.result);
        }
        reader.readAsText(file);
    }
    //支持IE 7 8 9 10  
    else if (typeof window.ActiveXObject != 'undefined') {
        var xmlDoc;
        xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
        xmlDoc.async = false;
        xmlDoc.load(input.value);
        editor.value = xmlDoc.xml;
    }
    //支持FF  
    else if (document.implementation && document.implementation.createDocument) {
        var xmlDoc;
        xmlDoc = document.implementation.createDocument("", "", null);
        xmlDoc.async = false;
        xmlDoc.load(input.value);
        editor.value = xmlDoc.xml;
    } else {
        codeText = "";
        alert('加载代码文件出现错误');
    }
} 

function complieResultTick() {
    $.ajax({
        url: "/script/selectScript",
        type: "post",
        dataType: "json",
        data: {
            session_id: id,
            compile_id: id
        },
        success: function (res) {
            outer.setValue(res.data.scriptDesc);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        }
    });  
}

function runResultTick() {

}