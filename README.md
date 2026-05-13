# DevKit CLI

Configure seu ambiente de desenvolvimento em um único comando.

```bash
npm install -g @igorlimadejesus/devkitcli
DevKit
```

---

## O que é?

DevKit é uma CLI interativa que detecta seu sistema operacional e instala automaticamente todas as ferramentas que você precisa para começar a desenvolver — sem abrir navegador, sem copiar comandos do Google.

## Instalação

Requer [Node.js 18+](https://nodejs.org).

```bash
npm install -g @igorlimadejesus/devkitcli
```

> O instalador baixa automaticamente o binário correto para o seu sistema operacional.

## Uso

```bash
DevKit
```

### Comandos

| Comando | Descrição |
|---|---|
| `setup` | Wizard interativo — pergunta sua stack e instala tudo |
| `install <ferramenta>` | Instala uma ferramenta específica |
| `install --profile <perfil>` | Instala um perfil completo |
| `list` | Lista todas as ferramentas disponíveis |
| `help` | Exibe os comandos disponíveis |
| `exit` | Sai do DevKit |

### Exemplos

```bash
# Wizard completo
DevKit setup

# Instalar uma ferramenta
DevKit install git
DevKit install docker

# Instalar por perfil
DevKit install --profile backend
DevKit install --profile frontend
```

## Ferramentas suportadas

| Categoria | Ferramentas |
|---|---|
| Editores | VS Code |
| Terminal | Windows Terminal |
| Controle de versão | Git |
| Runtimes | Node.js, .NET, Java (Temurin 21), Python, Go, Ruby, PHP |
| Bancos de dados | PostgreSQL, MySQL, SQL Server, MongoDB, MariaDB, Redis |
| Ferramentas de BD | DBeaver |
| API | Postman, Insomnia |
| Containers | Docker Desktop |

## Perfis

| Perfil | Ferramentas incluídas |
|---|---|
| `frontend` | VS Code, Git, Node.js, Docker |
| `backend` | VS Code, Git, Node.js, Docker, DBeaver, Postman |
| `fullstack` | Wizard completo de backend + frontend + banco de dados |
| `mobile` | VS Code, Git, Node.js |
| `devops` | Git, Docker |

## Plataformas suportadas

| Plataforma | Gerenciador de pacotes |
|---|---|
| Windows | winget |
| macOS | Homebrew (brew) |
| Linux | apt / snap |

## Construído com

- [.NET 10](https://dotnet.microsoft.com/) — runtime da CLI
- [Node.js](https://nodejs.org/) — instalador multiplataforma
