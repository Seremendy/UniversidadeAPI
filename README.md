#  UniversidadeAPI

API RESTful construída em ASP.NET Core 8.0 para o gerenciamento de dados acadêmicos de uma universidade (alunos, professores, cursos e turmas).

O projeto utiliza o padrão Repository, Dapper para acesso a dados de alta performance e JWT para autenticação segura.

---

###  Tecnologias Utilizadas (Tech Stack)

| Categoria | Tecnologia |
| :--- | :--- |
| **Backend** | ASP.NET Core 8.0 (LTS) |
| **Linguagem** | C# |
| **Banco de Dados** | MySQL |
| **Acesso a Dados** | Dapper |
| **Segurança** | JWT (JSON Web Tokens), BCrypt |
| **Documentação** | Swagger/OpenAPI |

---

###  Configuração e Setup Inicial

Siga os passos abaixo para configurar o ambiente e o banco de dados.

#### 1. Instalação de Pacotes NuGet 

Instale os seguintes pacotes para habilitar o acesso a dados e a segurança:

```bash
dotnet add package Dapper
dotnet add package MySqlConnector
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package BCrypt.Net-Next
````

#### 2\. Estrutura do Banco de Dados (MySQL) 

Crie o banco de dados e execute o script SQL abaixo.

```sql
-- ===============================================
-- SCRIPT DE CRIAÇÃO DO BANCO DE DADOS: universidadedb
-- ===============================================

CREATE DATABASE IF NOT EXISTS universidadedb;
USE universidadedb;

-- ----------------------------------------------------
-- 1. TABELAS PRINCIPAIS (ENTIDADES)
-- ----------------------------------------------------

-- Tabela de Departamentos (ex: Centro de Ciências Exatas)
CREATE TABLE Departamentos (
    DepartamentoID INT PRIMARY KEY AUTO_INCREMENT,
    DepartamentoNome VARCHAR(100) NOT NULL UNIQUE
);

-- Tabela de Salas de Aula
CREATE TABLE SalasDeAula (
    SalaDeAulaID INT AUTO_INCREMENT PRIMARY KEY,
    Capacidade INT NOT NULL,
    NumeroSala INT NOT NULL,
    PredioNome VARCHAR(50) NOT NULL
);

-- Tabela de Horários (Blocos de tempo)
CREATE TABLE Horarios (
    HorarioID INT PRIMARY KEY AUTO_INCREMENT,
    Dia ENUM('Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta') NOT NULL,
    HoraInicio TIME NOT NULL,
    HoraFim TIME NOT NULL
);

-- Tabela de Disciplinas (Matérias)
CREATE TABLE Disciplinas (
    DisciplinaID INT AUTO_INCREMENT PRIMARY KEY,
    NomeDisciplina VARCHAR(100) NOT NULL UNIQUE
);

-- Tabela de Alunos
CREATE TABLE Alunos (
    AlunoID INT PRIMARY KEY AUTO_INCREMENT,
    AlunoNome VARCHAR(150) NOT NULL,
    DataNascimento DATE NOT NULL,
    RG VARCHAR(9) NOT NULL UNIQUE,
    CPF VARCHAR(11) NOT NULL UNIQUE
);

-- Tabela de Professores
CREATE TABLE Professores (
    ProfessorID INT PRIMARY KEY AUTO_INCREMENT,
    ProfessorNome VARCHAR(150) NOT NULL,
    DataNascimento DATE NOT NULL,
    RG VARCHAR(9) NOT NULL UNIQUE,
    CPF VARCHAR(11) NOT NULL UNIQUE,
    Formacao VARCHAR(100)
);

-- Tabela de Usuários (Para Login e Autenticação JWT)
CREATE TABLE Usuarios (
    UsuarioID INT PRIMARY KEY AUTO_INCREMENT,
    Login VARCHAR(100) NOT NULL UNIQUE, 
    SenhaHash VARCHAR(255) NOT NULL, 
    Role ENUM('Admin', 'Professor', 'Aluno') NOT NULL
);


-- ----------------------------------------------------
-- 2. TABELAS DEPENDENTES E DE RELAÇÃO
-- ----------------------------------------------------

-- Tabela de Cursos (Graduações) - Depende de 'Departamentos'
CREATE TABLE Cursos (
    CursoID INT AUTO_INCREMENT PRIMARY KEY,
    NomeCurso VARCHAR(100) NOT NULL UNIQUE,
    DepartamentoID INT NOT NULL,
    FOREIGN KEY (DepartamentoID) REFERENCES Departamentos(DepartamentoID)
);

-- Relação Curso <-> Disciplina (Grade Curricular)
CREATE TABLE GradeCurricular (
    GradeCurricularID INT AUTO_INCREMENT PRIMARY KEY,
    DisciplinaID INT NOT NULL,
    FOREIGN KEY (DisciplinaID) REFERENCES Disciplinas(DisciplinaID),
    CursoID INT NOT NULL,
    FOREIGN KEY (CursoID) REFERENCES Cursos(CursoID)
);

-- Relação Disciplina <-> Disciplina (Pré-Requisitos)
CREATE TABLE PreRequisitos (
    DisciplinaID INT NOT NULL,
    PreRequisitoID INT NOT NULL,
    PRIMARY KEY (DisciplinaID, PreRequisitoID), 
    FOREIGN KEY (DisciplinaID) REFERENCES Disciplinas(DisciplinaID),
    FOREIGN KEY (PreRequisitoID) REFERENCES Disciplinas(DisciplinaID)
);

-- Relação Aluno <-> Curso (Matrícula)
CREATE TABLE Matricula (
    MatriculaID INT AUTO_INCREMENT PRIMARY KEY,
    AlunoID INT NOT NULL,
    FOREIGN KEY (AlunoID) REFERENCES Alunos(AlunoID),
    CursoID INT NOT NULL,
    FOREIGN KEY (CursoID) REFERENCES Cursos(CursoID),
    DataMatricula DATE NOT NULL,
    MatriculaAtiva BOOLEAN NOT NULL DEFAULT TRUE
);

-- Relação Aluno <-> Disciplina (Notas)
CREATE TABLE Notas (
    NotaID INT AUTO_INCREMENT PRIMARY KEY,
    Nota DECIMAL(4, 2) NOT NULL,
    AlunoID INT NOT NULL,
    FOREIGN KEY (AlunoID) REFERENCES Alunos(AlunoID),
    DisciplinaID INT NOT NULL,
    FOREIGN KEY (DisciplinaID) REFERENCES Disciplinas(DisciplinaID)
);

-- Tabela 'Turma' (O evento da aula) - Junção de 4 entidades
CREATE TABLE Turma (
    TurmaID INT AUTO_INCREMENT PRIMARY KEY,
    Semestre VARCHAR(10) NOT NULL,
    DisciplinaID INT NOT NULL,
    FOREIGN KEY (DisciplinaID) REFERENCES Disciplinas(DisciplinaID),
    SalaDeAulaID INT NOT NULL,
    FOREIGN KEY (SalaDeAulaID) REFERENCES SalasDeAula(SalaDeAulaID),
    ProfessorID INT NOT NULL,
    FOREIGN KEY (ProfessorID) REFERENCES Professores(ProfessorID),
    HorarioID INT NOT NULL,
    FOREIGN KEY (HorarioID) REFERENCES Horarios(HorarioID)
);
```

#### 3\. Configuração do `appsettings.json` 

Configure a string de conexão e os parâmetros do JWT. **Substitua os placeholders pelos seus valores secretos.**

```json
{
  "Logging": { /* ... */ },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=universidadedb;User Id=root;Password=SUA_SENHA_MYSQL;"
  },
  "Jwt": {
    "Key": "SUA_CHAVE_SECRETA_MUITO_LONGA_E_COMPLEXA", 
    "Issuer": "UniversidadeAPI",
    "Audience": "UniversidadeAppUsuarios"
  }
}
```

-----

###  Setup de Segurança: Usuário Admin Inicial

O primeiro usuário 'Admin' precisa ser inserido manualmente para gerar tokens e usar as rotas protegidas.

1.  **Gere o Hash da Senha em C\#:**

      * Use o trecho de código abaixo para gerar o hash da senha (ex: `123456`).

    <!-- end list -->

    ```csharp
    using BCrypt.Net; 
    string senhaOriginal = "123456";
    string senhaHash = BCrypt.Net.BCrypt.HashPassword(senhaOriginal);
    Console.WriteLine(senhaHash); // Copie esta string
    ```

2.  **Execute o INSERT no MySQL:**

      * Insira o usuário na tabela `Usuarios`, usando o hash gerado e definindo a `Role` como 'Admin'.

    <!-- end list -->

    ```sql
    INSERT INTO Usuarios (Login, SenhaHash, Role)
    VALUES ('admin', 'COLE_O_HASH_GERADO_AQUI', 'Admin');
    ```

-----

###  Como Rodar e Testar a API

1.  Abra o projeto no Visual Studio.
2.  Pressione **F5** (Debug) ou **Ctrl+F5** (Sem Debug).
3.  A interface do **Swagger UI** será aberta.

#### Fluxo de Teste:

1.  Acesse **`POST /api/Auth/login`** com as credenciais `admin` / `123456` para obter o Token JWT.
2.  Use o botão **Authorize** no Swagger para colar o Token (formato `Bearer [Token]`).
3.  Teste as rotas protegidas (ex: `POST /api/Departamentos`).

###  Endpoints Principais (Controlador de Departamentos)

| Ação | Método | Rota | Descrição | Restrição |
| :--- | :---: | :--- | :--- | :---: |
| **Login** | `POST` | `/api/Auth/login` | Gera o JWT de acesso. | Pública |
| **Registro** | `POST` | `/api/Auth/register` | Cria um novo usuário com. | Pública(Token Requerido) | 
| **Listar Deptos** | `GET` | `/api/Departamentos` | Retorna a lista completa. | Token Requerido |
| **Criar Depto** | `POST` | `/api/Departamentos` | Cria um novo departamento. | **Admin** |
| **Atualizar Depto** | `PUT` | `/api/Departamentos/{id}` | Atualiza um departamento. | **Admin** |
| **Deletar Depto** | `DELETE` | `/api/Departamentos/{id}` | Deleta um departamento pelo ID. | **Admin** |

 ->Registro<- Apenas para criar seu usuario admin depois você precisa colocar [Authorize], para que fique seguro na parte de registro.
-----

###  Novas Atualizações (Roadmap)

#### Próximas Etapas (Prioridade) 

  * Implementar o CRUD completo para a entidade `Cursos` (Modelos, Repositório e Controller).
  * Criar o método protegido (`PUT /api/Auth/promote/{id}`) para que um **Admin** possa alterar a `Role` de um usuário.

#### Melhorias Futuras 

  * Desenvolver a lógica de `Matrícula` (`Aluno` em `Curso`).
  * Adicionar suporte para Paginação Global (`?page=...`).

-----

1.  **Testar a API** (Inserir o usuário 'admin' no DB e testar o fluxo completo).
2.  **Nova Entidade**  (Começar o CRUD de `Cursos`).
3.  **Lógica de Promoção** (Criar o endpoint para Admin promover a Role de outro usuário).
