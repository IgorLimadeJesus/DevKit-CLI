# DevKit CLI v1.0.0

Configure seu ambiente de desenvolvimento em um único comando.

## O que é o DevKit?

DevKit é uma CLI interativa que detecta seu sistema operacional e instala automaticamente todas as ferramentas que você precisa para começar a desenvolver — sem abrir navegador, sem copiar comandos do Google.

## Funcionalidades

- **Wizard interativo** — responde algumas perguntas sobre sua stack e o DevKit instala tudo
- **Perfis prontos** — `frontend`, `backend`, `fullstack`, `mobile`, `devops`
- **Cross-platform** — funciona no Windows (winget), macOS (brew) e Linux (apt/snap)
- **Detecção de instalação** — pula ferramentas que já estão instaladas

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

## Instalação

```bash
npm install -g @igorlimadejesus/devkitcli
```

## Uso

```bash
DevKit
```

### Comandos disponíveis

```
setup                        Wizard interativo completo
install <ferramenta>         Instala uma ferramenta específica
install --profile <perfil>   Instala um perfil completo
list                         Lista todas as ferramentas disponíveis
help                         Exibe os comandos disponíveis
exit                         Sai do DevKit
```

### Perfis disponíveis

```
frontend   VS Code, Git, Node.js, Docker
backend    VS Code, Git, Node.js, Docker, DBeaver, Postman
mobile     VS Code, Git, Node.js, Android Studio
devops     Git, Docker, Terraform, kubectl
```

## Plataformas suportadas

| Plataforma | Gerenciador |
|---|---|
| Windows | winget |
| macOS | Homebrew (brew) |
| Linux | apt / snap |
