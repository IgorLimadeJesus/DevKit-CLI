const https = require("https");
const fs = require("fs");
const path = require("path");
const { execSync } = require("child_process");

const GITHUB_USER = "IgorLimadeJesus";
const GITHUB_REPO = "DevKit-CLI";
const RELEASE_VERSION = "1.2.0";

const pkg = require("./package.json");
const version = RELEASE_VERSION;

function getPlatformAsset() {
    const { platform, arch } = process;

    const map = {
        "win32-x64": "devkit-win-x64.exe",
        "darwin-x64": "devkit-osx-x64",
        "darwin-arm64": "devkit-osx-arm64",
        "linux-x64": "devkit-linux-x64",
        "linux-arm64": "devkit-linux-arm64",
    };

    const key = `${platform}-${arch}`;
    const asset = map[key];

    if (!asset) {
        console.error(`[DevKit] Plataforma não suportada: ${key}`);
        process.exit(1);
    }

    return asset;
}

function download(url, dest) {
    return new Promise((resolve, reject) => {
        const file = fs.createWriteStream(dest);

        const request = (targetUrl) => {
            https.get(targetUrl, (res) => {
                if (res.statusCode === 302 || res.statusCode === 301) {
                    request(res.headers.location);
                    return;
                }
                if (res.statusCode !== 200) {
                    reject(new Error(`HTTP ${res.statusCode} ao baixar ${targetUrl}`));
                    return;
                }
                res.pipe(file);
                file.on("finish", () => file.close(resolve));
            }).on("error", (err) => {
                fs.unlink(dest, () => { });
                reject(err);
            });
        };

        request(url);
    });
}

async function main() {
    const asset = getPlatformAsset();
    const url = `https://github.com/${GITHUB_USER}/${GITHUB_REPO}/releases/download/v${version}/${asset}`;
    const binDir = path.join(__dirname, "bin");
    const isWin = process.platform === "win32";
    const binName = isWin ? "devkit-bin.exe" : "devkit-bin";
    const dest = path.join(binDir, binName);

    if (!fs.existsSync(binDir)) fs.mkdirSync(binDir, { recursive: true });

    console.log(`[DevKit] Baixando ${asset} para ${process.platform}...`);

    try {
        await download(url, dest);
        if (!isWin) fs.chmodSync(dest, 0o755);
        console.log("[DevKit] Instalado com sucesso. Use 'DevKit' para começar.");
    } catch (err) {
        console.error(`[DevKit] Falha ao baixar binário: ${err.message}`);
        console.error(`[DevKit] URL: ${url}`);
        process.exit(1);
    }
}

main();
