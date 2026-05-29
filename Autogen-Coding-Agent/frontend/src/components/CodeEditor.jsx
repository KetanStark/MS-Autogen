import Editor from "@monaco-editor/react";

export default function CodeEditor({
    code,
    setCode
}) {
    return (
        <Editor
            height="90vh"
            defaultLanguage="csharp"
            value={code}
            onChange={setCode}
        />
    );
}