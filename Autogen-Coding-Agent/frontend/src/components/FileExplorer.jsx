export default function FileExplorer({
    files,
    feFiles,
    onSelect
}) {
    return (
        <div>

            {files && files.length > 0 && (
                <>
                    <div>
                        <b>BackEnd Files</b>
                    </div>

                    <ul className="file-list">
                    {files.map(file => (
                        console.log(file),
                        console.log(file.split('\\').pop()),
                        <div
                            key={file}
                            onClick={() => onSelect(file)}
                            style={{
                                cursor: "pointer",
                                padding: 5
                            }}
                        >
                            {file.indexOf("\\") > 0 ? file.split('\\').pop() : file}
                        </div>

                    ))}
                    </ul>
                </>
            )}
            {feFiles && feFiles.length > 0 && (
                <>
                    <div><br /><b>Fronend files</b></div>

                    <ul className="file-list">
                    {feFiles.map(file => (
                        <div
                            key={file}
                            onClick={() => onSelect(file)}
                            style={{
                                cursor: "pointer",
                                padding: 5
                            }}
                        >
                              {file.indexOf("\\") > 0 ? file.split('\\').pop() : file}
                        </div>
                    ))}
                    </ul>
                </>
            )}

        </div>
    );
}