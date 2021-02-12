# POC-Asp.NET-Core-Identity

## Introdu��o

Este � o arquivo README (leia-me) contendo as informa��es b�sicas de como utilizar o c�digo-fonte do projeto, alterar, compilar e executar a api.

Este projeto � uma poc API com Asp.NET Core Identity + Entity Frammework Core + JWT + Sql Server onde iremos construir um servi�o de autentica��o e autoriza��o de usu�rio(Operador e Cliente), cadastro de veiculos e um pdf sem formata��o para entendimento de como seria gerar o pdf em uma api com DinkToPdf Library.

### Principais ferramentas
  
* Visual Studio: [Visual Studio 2019](https://my.visualstudio.com/Downloads/Featured)
  
* Visual Studio Code (VS Code): [VS Code](https://code.visualstudio.com/)

* Sql Server Express: [Sql Server](https://www.microsoft.com/en-us/download/details.aspx?id=101064)

* JWT [Jwt](https://jwt.io/)

## Setup do ambiente de desenvolvimento

### Clonando o reposit�rio

1) Acesse o reposit�rio no [GitHub](https://github.com/vitoranalistati/POC-Asp.NET-Core-Identity);
2) Clique em 'Clone'  =>  'Clone in VS Code'  =>  Caso abra uma janela escolha 'Open Visual Studio Code';
3) Selecione uma pasta para o download do reposit�rio;
4) Abra o reposit�rio no Visual Studio ou VS Code.

### Instalando depend�ncias

A instala��o das depend�ncias � feita de forma autom�tica.

1) AutoMapper
2) DinkToPdf
3) Microsoft.AspNetCore.App
4) Microsoft.AspNetCore.Authentication.JwtBearer
5) Microsoft.VisualStudio.Web.CodeGeneration.Design  
6) Microsoft.EntityFrameworkCore.Design
7) Microsoft.EntityFrameworkCore.SqlServer
8) Microsoft.AspNetCore.Identity.EntityFrameworkCore
9) Microsoft.EntityFrameworkCore.Tools
10) Microsoft.Extensions.Identity.Core  
11) Swashbuckle.AspNetCore     

### Alterando ConnectionString Banco de Dados no projeto

Crie uma inst�ncia de banco de dados local:
* Exemplo: [Como criar inst�ncia de banco de dados](https://pt.wikihow.com/Criar-um-Banco-de-Dados-SQL-Server) 

Abra o arquivo appsettings.json do projeto e altere o DefaultConnection para sua conex�o local.

### Criando Banco de Dados

Abra o windows explorer no diret�rio raiz do projeto WebApi.Identity(Bot�o direito no projeto WebApi.Identity e open folder in file explorer)

Na raiz iremos abrir o powershell, bot�o direito com o shift pressionado.

Criando Migrations
```powershell
dotnet ef migrations add initial
```
em seguida criando BD:

```powershell
dotnet ef database update
```

Adicionei o arquivo QueryValidacao.sql com selects nas tabelas base.

## Testes automatizados

### Testes unit�rios

O projeto foi criado, mas infelizmente n�o houve tempo para conclus�o ainda.

## Postman ou Swagger

Adicionei o arquivo Postman_collection.json para quem quiser importar no Postman, al�m disso foi configurado o swagger.

# Fim