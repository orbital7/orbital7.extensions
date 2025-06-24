export function copyTextToClipboard(text) {

    if (navigator.clipboard) {
        return navigator.clipboard.writeText(text);
    } else {
        // fallback for older browsers
        const textarea = document.createElement("textarea");
        textarea.value = text;
        document.body.appendChild(textarea);
        textarea.select();
        document.execCommand("copy");
        document.body.removeChild(textarea);
        return Promise.resolve();
    }
}

export function pasteTextFromClipboard() {

    if (navigator.clipboard) {
        return navigator.clipboard.readText();
    } else {
        // fallback for older browsers
        return new Promise((resolve, reject) => {
            const textarea = document.createElement("textarea");
            document.body.appendChild(textarea);
            textarea.focus();
            document.execCommand("paste");
            const text = textarea.value;
            document.body.removeChild(textarea);
            resolve(text);
        });
    }
}