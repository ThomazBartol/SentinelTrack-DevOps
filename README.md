# SentinelTrack API

**SentinelTrack** é uma API REST desenvolvida em .NET 8 com Entity Framework Core e banco de dados SQL Server em nuvem.
Essa API consiste em gerenciar Usuários, Leituras feitas por sensores em nossos SafeCaps (Bonés Inteligentes), e também Alertas baseados nessas leituras.

Nossa solução ajuda a previnir acidentes por desastres climaticos, controlando o nível de humidade, temperatura e luminosidade, alertando os usuários quando
esses níveis pudessem atingir valores prejudiciais.

---

## Passo a passo para fazer o deploy

### Criando o resource group
az group create \
  --name "rg-sentineltrack" \
  --location "eastus2"

### Criando Servidor SQL
az sql server create \
  --name "server-sentineltrack" \
  --resource-group "rg-sentineltrack" \
  --location "eastus2" \
  --admin-user "sqladmin" \
  --admin-password "AzureDb123"

### Criando o banco de dados Azure SQL
az sql db create \
  --resource-group "rg-sentineltrack" \
  --server "server-sentineltrack" \
  --name "db-sentineltrack" \
  --service-objective S0

### Permitindo o WebApp acessar o banco
az sql server firewall-rule create \
  --resource-group "rg-sentineltrack" \
  --server "server-sentineltrack" \
  --name AllowAzureIps \
  --start-ip-address 0.0.0.0 \
  --end-ip-address 0.0.0.0

### Criando o App Service Plan
az appservice plan create \
  --name "plan-sentineltrack" \
  --resource-group "rg-sentineltrack" \
  --sku F1

### Criando o Web App de fato com Dotnet
az webapp create \
  --resource-group "rg-sentineltrack" \
  --plan "plan-sentineltrack" \
  --name "app-sentineltrack" \
  --runtime "dotnet:8"

### Conection String
CONNECTION_STRING="Server=tcp:server-sentineltrack.database.windows.net,1433;Initial Catalog=db-sentineltrack;Persist Security Info=False;User ID=sqladmin;Password=AzureDb123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

### Inserindo a Conection String
az webapp config connection-string set \
  --resource-group "rg-sentineltrack" \
  --name "app-sentineltrack" \
  --settings DefaultConnection="$CONNECTION_STRING" \
  --connection-string-type SQLAzure

### Aplicando as configs
az webapp deployment source config \
    --name app-sentineltrack \
    --resource-group rg-sentineltrack \
    --repo-url https://$app-sentineltrack@app-sentineltrack.scm.azurewebsites.net/app-sentineltrack.git \
    --branch main \
    --manual-integration

### Permitindo o ip para edição no banco
az sql server firewall-rule create \
    --resource-group rg-sentineltrack \
    --server server-sentineltrack \
    --name AllowMyIP \
    --start-ip-address {seu ip} \
    --end-ip-address {seu ip}

### Criando as tabelas
dotnet ef database update

### Fazer o deploy de fato
git remote add azure https://$app-sentineltrack@app-sentineltrack.scm.azurewebsites.net/app-sentineltrack.git

### Vendo o user e senha
az webapp deployment list-publishing-profiles \
  --resource-group "rg-sentineltrack" \
  --name "app-sentineltrack" \
  --output table

### Fazendo o Push
git push azure main

---

## Rotas Disponíveis

---

### Usuários (`/api/users`)

- **GET /api/users** — Lista os Usuários, com filtros opcionais via query params:

  | Query Param  | Tipo    | Descrição                                    | Exemplo         |
  |--------------|---------|----------------------------------------------|-----------------|
  | name         | string  | Filtra os usuários pelo nome de usuário      | `/api/users?name=Gabriel` |
  | email        | string  | Filtra os usuários pelo email                | `/api/users?email=gabriel@gmail.com` |

- **GET /api/users/{id}** — Busca usuário pelo ID.

- **POST /api/users** — Cria um novo usuário.

- **PUT /api/users/{id}** — Atualiza um usuário existente.

- **DELETE /api/users/{id}** — Remove um usuário.

---

### Leituras do Sensor (`/api/readings`)

- **GET /api/readings** — Lista as Leituras, com filtros opcionais via query params:

  | Query Param  | Tipo    | Descrição                                    | Exemplo         |
  |--------------|---------|----------------------------------------------|-----------------|
  | userId       | Guid    | Filtra as leituras pelo id do usuário        | `/api/readings?userId=...` |
  | startDate    | DateTime | Filtra as leituras após uma data            | `/api/readings?startDate=2025-06-07` |
  | endDate      | DateTime | Filtra as leituras antes de uma data        | `/api/readings?endDate=2025-06-07` |

- **GET /api/readings/{id}** — Busca leitura pelo ID.

- **POST /api/readings** — Cria uma nova leitura.

- **PUT /api/readings/{id}** — Atualiza uma leitura existente.

- **DELETE /api/readings/{id}** — Remove uma leitura.

---

### Alertas (`/api/alerts`)

- **GET /api/alerts** — Lista os Alertas, com filtros opcionais via query params:

  | Query Param  | Tipo    | Descrição                                    | Exemplo         |
  |--------------|---------|----------------------------------------------|-----------------|
  | userId       | Guid    | Filtra os alertas pelo id do usuário         | `/api/alerts?userId=...` |
  | alertType    | string  | Filtra os alertas pelo tipo de alerta        | `/api/alerts?alertType=LowHumidity` |

- **GET /api/alerts/{id}** — Busca alerta pelo ID.

- **POST /api/alerts** — Cria um novo alerta.

- **PUT /api/alerts/{id}** — Atualiza um alerta existente.

- **DELETE /api/alerts/{id}** — Remove um alerta.

---

## Instruções de Execução

1. Clone o repositório:
   ```bash
   git clone https://github.com/ThomazBartol/SafeCapAPI.git
   cd SafeCapAPI/

2. Restore as dependências com:
   ```bash
   dotnet restore

3. Faça o build da aplicação com:
   ```bash
   dotnet build   

4. Rode o projeto com o comando:
   ```bash
   dotnet run

5. Caso o Swagger não abra sozinho acesse em:
   http://localhost:8080/swagger/index.html

---

## Comandos e Scripts Testes da API

### Json para POST de Usuário:
{
    "name": "UsuarioPadrao",
    "email": "user@example.com"
}

### Json para PUT de Usuário:
{
  "name": "UsuarioAtualizado",
  "email": "user@example.com"
}

### Json para POST de Leituras do Sensor:
{
  "userId": "679e0ef4-a2e1-4a19-d6d3-08de0141bbb4",
  "temperature": 12,
  "humidity": 43,
  "light": 32
}

### Json para PUT de Leituras do Sensor:
{
  "temperature": 10,
  "humidity": 15,
  "light": 32
}

### Json para POST de Alertas:
{
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "alertType": "string",
  "message": "string"
}

### Json para PUT de Alertas:
{
  "alertType": "Temperatura",
  "message": "Temperatura acima do comum"
}

### Scripts SQL para alterações diretamente no banco

--ver todas as tabelas
SELECT TABLE_SCHEMA, TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_SCHEMA, TABLE_NAME;

--insert na tabela de Alertas
INSERT INTO SC_Alerts (Id, UserId, AlertType, Message, Timestamp)
VALUES (NEWID(), 
        '3fa85f64-5717-4562-b3fc-2c963f66afa6', 
        'string', 
        'string',
    '2025-10-01T23:30:56.628Z');

--update na tabela de Alertas
UPDATE SC_Alerts
SET Message = 'Nova mensagem',
    AlertType = 'novo_tipo'
WHERE UserId = '3fa85f64-5717-4562-b3fc-2c963f66afa6';

--delete na tabela de Alertas
UPDATE SC_Alerts
WHERE UserId = '3fa85f64-5717-4562-b3fc-2c963f66afa6';

---

## Desenho da Arquitetura

<img src="diagrams/img/diagram.png" alt="Class Diagram" width="400"/>


## 👥 INTEGRANTES DO GRUPO
===========================

- RM555323 - Thomaz Oliveira Vilas Boas Bartol
- RM556089 - Vinicius Souza Carvalho
- RM556972 - Gabriel Duarte Pinto
