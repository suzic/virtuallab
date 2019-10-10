
$(document).ready(function () {
    var editor = CodeMirror.fromTextArea(document.getElementById('code_text'),
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
    var outer = CodeMirror.fromTextArea(document.getElementById('debug_text'),
        {
            mode: 'textile',
            theme: 'zenburn',
            identUnit: 4,
            readOnly: true
        });
    outer.setSize('100%', '100%');
});

function JSAction() {
    alert("JS Worked!");
}