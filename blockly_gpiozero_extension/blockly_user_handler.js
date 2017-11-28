function generateCode() {
    saveToFile(Blockly.Python.workspaceToCode(demoWorkspace), 'pythonCodeFromBlocks.py');
}

function saveToFile(textToSave, fileName) {
    var hiddenElement = document.createElement('a');
    hiddenElement.href = 'data:attachment/text,' + encodeURI(textToSave);
    hiddenElement.target = '_blank';
    hiddenElement.download = fileName;
    hiddenElement.click();
}

function saveBlocks() {
    var xml = Blockly.Xml.workspaceToDom(demoWorkspace);
    var xml_text = Blockly.Xml.domToText(xml);
    saveToFile(xml_text, 'blocksToSave.xml');
}

function readSingleFile(e) {
    var file = e.target.files[0];
    if (!file) {
        return;
    }
    var reader = new FileReader();
    reader.onload = function (e) {
        var contents = e.target.result;
        updateWorkspaceWithBlocks(contents)
    };
    reader.readAsText(file);
}

function connectSSH(ipAndPort) {
    var http = new XMLHttpRequest();
    var url = 'http://' + ipAndPort + '/fileupload';
    var body = Blockly.Python.workspaceToCode(demoWorkspace);
    http.open("POST", url, true);

    //Send the proper header information along with the request
    http.setRequestHeader("Content-type", "application/x-www-form-urlencoded");

    http.onreadystatechange = function () {
        if (http.readyState === 4 && http.status === 200) {
            alert(http.responseText);
        }
    };

    http.send(body);
}