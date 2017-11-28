function updateWorkspaceWithBlocks(blocks) {
    var xml = Blockly.Xml.textToDom(blocks);
    Blockly.mainWorkspace.clear();
    Blockly.Xml.domToWorkspace(xml, demoWorkspace);
}

function updateRealTimeBlockToPythonConversionTextarea(event) {
    document.getElementById('on-the-fly-code-textarea').value = Blockly.Python.workspaceToCode(demoWorkspace);
}