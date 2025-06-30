
Para rodar o projeto, é necessário instalar o Dotnet.
-https://dotnet.microsoft.com/pt-br/download/dotnet/thank-you/sdk-8.0.411-windows-x64-installer

É necessário ter um servidor Mysql configurado, com o usuario root, sem senha. Criar um database chamado projetobk

Abrir a pasta do projeto e rodar os seguintes comandos no terminal:
dotnet tool install --global dotnet-ef

dotnet ef migrations add NomeDaMigracao
dotnet ef database update

após isso, ainda na pasta do projeto, rodar o comando dotnet run.

Feito isso, o projeto deve estar rodando. Confirme no terminal a porta que o projeto está rodando e acesse via localhost no navegador.

EX: localhost:5000 ou localhost:5001.

