# Projeto de Blog Simples

## Descrição

Este é um sistema básico de blog desenvolvido em C# utilizando .NET 6, com suporte a autenticação de usuários, criação, edição e exclusão de postagens. Implementa Entity Framework para manipulação de dados e WebSockets para notificações em tempo real.

## Pré-requisitos

Para executar este projeto, você precisará:

- .NET 6
- MongoDB instalado e em execução na sua máquina
- Um IDE compatível, como Visual Studio 2019 ou superior, ou VSCode

## Configuração

Siga os passos abaixo para configurar o projeto em seu ambiente:

1. Clone o repositório para sua máquina local usando:

```
git clone URL_DO_REPOSITORIO
```


2. Abra a solução do projeto no seu IDE.

3. Instale os pacotes NuGet necessários:
- Entity Framework Core
  ```
  dotnet add package Microsoft.EntityFrameworkCore --version ULTIMA_VERSAO
  ```
- MongoDB Driver
  ```
  dotnet add package MongoDB.Driver --version ULTIMA_VERSAO
  ```
- Newtonsoft.Json
  ```
  dotnet add package Newtonsoft.Json --version ULTIMA_VERSAO
  ```
- WebSocketManager
  ```
  dotnet add package WebSocketManager --version ULTIMA_VERSAO
  ```

4. Configure a string de conexão do MongoDB no arquivo de configuração `appsettings.json`.

5. Execute as migrações do Entity Framework para criar o banco de dados:
```
dotnet ef migrations add InitialCreate

dotnet ef database update
```
6. Inicie o projeto:
   ```
    dotnet run
   ```

## Uso

Após iniciar o projeto, você pode:

- Registrar e autenticar usuários
- Criar, editar e deletar postagens
- Visualizar todas as postagens
- Receber notificações em tempo real quando novas postagens são criadas




