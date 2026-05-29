import { useEffect, useState } from "react";

import api from "./api";

import FileExplorer
    from "./components/FileExplorer";

import CodeEditor
    from "./components/CodeEditor";

function App() {

    const [files, setFiles] = useState([]);
    const [feFiles, setFeFiles] = useState([]);

    const [selected, setSelected] = useState("");

    const [code, setCode] = useState("");


    const [instruction, setInstruction] =
        useState("");

    const [loading, setLoading] = useState(false); // State for loader


    useEffect(() => {
        api.get("/files")
            .then(r => {
                debugger;
                setFiles(r.data.files);
                setFeFiles(r.data.feFile);
            });

    }, []);

    async function openFile(path) {
        if (path != null) {
            setSelected(path);

            const r =
                await api.get(
                    "/files/content",
                    {
                        params: { path }
                    });

            setCode(r.data);
        } else {
            alert("select file path");
        }
    }

    async function modifyCode() {
        setLoading(true); // Show loader
        try {
            const r =
                await api.post(
                    "/agent/modify",
                    {
                        path: selected,
                        instruction
                    });

            setCode(r.data);
        } catch (error) {
            console.error("Error modifying code:", error);
        } finally {
            setLoading(false); // Hide loader
        }
    }

    return (

        <div style={{
            display: "flex",
            marginTop : "30px"
        }}>

            <div style={{
                width: 300,
                borderRight: "1px solid #ccc"
            }}>

                <FileExplorer
                    files={files}
                    feFiles={feFiles}
                    onSelect={openFile}
                />

            </div>

            <div style={{
                flex: 1
            }}>

                <textarea
                    rows={4}
                    style={{
                        width: "99%"
                    }}
                    placeholder="AI Instruction"
                    onChange={e =>
                        setInstruction(
                            e.target.value
                        )
                    }
                />

                <button
                    className="btn"
                    onClick={modifyCode}
                    disabled={loading} // Disable button while loading
                >
                    {loading ? "Processing..." : "Run AI"}
                </button>

                {loading && (
                    <div className="loader">
                        Loading...
                    </div>
                )}

                <CodeEditor
                    code={code}
                    setCode={setCode}
                />

            </div>

        </div>
    );
}

export default App;
