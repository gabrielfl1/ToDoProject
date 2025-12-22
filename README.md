# ToDo API – ASP.NET Core

Pequeno projeto desenvolvido em **ASP.NET Core** para gerenciamento de tarefas, utilizando boas praticas de desenvolvimento utilizadas no mercado

Projeto criado para fins de **portfólio**.

Projeto criado para fins de **portfólio**.

---

## Tecnologias

- ASP.NET Core
- Entity Framework Core
- SQLite
- LINQ
- Swagger

---

## Funcionalidades

- CRUD completo de tarefas
- Paginação
- Filtro por status (`IsCompleted`)
- Filtro por prioridade (`Priority`)
- Validação de dados com DataAnnotations
- Padronização de respostas da API

---

## Padrão de Resposta

```json
{
  "data": {},
  "errors": []
}
```

---

## Rota principal

### Buscar tarefas
`GET /v1/todos`

**Query params:**
- `page` (obrigatório)
- `pageSize` (obrigatório)
- `isCompleted`
  - `0` → não concluídas
  - `1` → concluídas
  - `2` → todas
- `priority`
  - `0` → todas
  - `1 a 5` → prioridade específica

---

## Banco de Dados

- SQLite
- Criado via Entity Framework Core Migrations
- Arquivo do banco não versionado no repositório

---

## Como Executar

```bash
git clone https://github.com/gabrielfl1/ToDoProject.git
cd ToDoProject
dotnet ef database update
dotnet run
```

Acesse o Swagger em:
```
https://localhost:{porta}/swagger
```
Ou se preferir baixe a collection do postman neste link:  
[postman collection.json](https://github.com/gabrielfl1/ToDoProject/blob/main/Postman/ToDo%20Collection.postman_collection.json)

---

## Autor

Gabriel Ferreira Lima  
Projeto de portfólio para demonstração de conhecimento em APIs com .NET
