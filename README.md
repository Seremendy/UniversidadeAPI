# UniversidadeAPI

API RESTful construída em ASP.NET Core 8.0 para o gerenciamento de dados acadêmicos de uma universidade (alunos, professores, cursos e turmas).

O projeto utiliza o padrão Repository, Dapper para acesso a dados de alta performance, DTOs para contratos de API seguros e JWT para autenticação baseada em Roles.

---

### Features Principais

* **Autenticação JWT Completa**: Login de usuários com tokens JWT e senhas protegidas com BCrypt.
* **Controle de Acesso por Roles**: Endpoints protegidos por autorização (`[Authorize]`) e restritos por Roles (`Admin`, `Professor`, `Aluno`).
* **Arquitetura Limpa**: Separação clara de responsabilidades (SOLID) usando:
    * **Controllers**: Orquestração de API e validação.
    * **DTOs**: Contratos de API (Request/Response) para segurança.
    * **Repositórios**: Camada de acesso a dados (Dapper) desacoplada com interfaces.
    * **Entidades**: Modelos que representam o esquema do banco.
* **CRUD Completo**: API com endpoints para todas as 13 entidades do sistema.

### Tecnologias Utilizadas (Tech Stack)

| Categoria | Tecnologia |
| :--- | :--- |
| **Backend** | ASP.NET Core 8.0 (LTS) |
| **Linguagem** | C# |
| **Banco de Dados** | MySQL |
| **Acesso a Dados** | Dapper |
| **Segurança** | JWT (JSON Web Tokens), BCrypt |
| **Documentação** | Swagger/OpenAPI |

---

### Configuração e Setup Inicial

Siga os passos abaixo para configurar o ambiente e o banco de dados.

#### 1. Instalação de Pacotes NuGet

Instale os seguintes pacotes para habilitar o acesso a dados e a segurança:

```bash
dotnet add package Dapper
dotnet add package MySqlConnector
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package BCrypt.Net-Next
```

### Crie o banco de dados e execute o script SQL abaixo para criar todo o schema com nomes de tabela no singular.

```bash
-- ===============================================
-- SCRIPT DE CRIAÇÃO DO BANCO DE DADOS: universidadedb
-- ===============================================

CREATE DATABASE IF NOT EXISTS universidadedb;
USE universidadedb;

-- ----------------------------------------------------
-- 1. TABELAS PRINCIPAIS (ENTIDADES)
-- ----------------------------------------------------

CREATE TABLE Departamento (
    DepartamentoID INT PRIMARY KEY AUTO_INCREMENT,
    DepartamentoNome VARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE SalaDeAula (
    SalaDeAulaID INT AUTO_INCREMENT PRIMARY KEY,
    Capacidade INT NOT NULL,
    NumeroSala INT NOT NULL,
    PredioNome VARCHAR(50) NOT NULL
);

CREATE TABLE Horario (
    HorarioID INT PRIMARY KEY AUTO_INCREMENT,
    DiaSemana ENUM('Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta') NOT NULL,
    HoraInicio TIME NOT NULL,
    HoraFim TIME NOT NULL
);

CREATE TABLE Disciplina (
    DisciplinaID INT AUTO_INCREMENT PRIMARY KEY,
    NomeDisciplina VARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE Aluno (
    AlunoID INT PRIMARY KEY AUTO_INCREMENT,
    AlunoNome VARCHAR(150) NOT NULL,
    DataNascimento DATE NOT NULL,
    RG VARCHAR(9) NOT NULL UNIQUE,
    CPF VARCHAR(11) NOT NULL UNIQUE
);

CREATE TABLE Professor (
    ProfessorID INT PRIMARY KEY AUTO_INCREMENT,
    ProfessorNome VARCHAR(150) NOT NULL,
    DataNascimento DATE NOT NULL,
    RG VARCHAR(9) NOT NULL UNIQUE,
    CPF VARCHAR(11) NOT NULL UNIQUE,
    Formacao VARCHAR(100)
);

CREATE TABLE Usuario (
    UsuarioID INT PRIMARY KEY AUTO_INCREMENT,
    Login VARCHAR(100) NOT NULL UNIQUE, 
    SenhaHash VARCHAR(255) NOT NULL, 
    Role ENUM('Admin', 'Professor', 'Aluno') NOT NULL
);


-- ----------------------------------------------------
-- 2. TABELAS DEPENDENTES E DE RELAÇÃO
-- ----------------------------------------------------

CREATE TABLE Curso (
    CursoID INT AUTO_INCREMENT PRIMARY KEY,
    NomeCurso VARCHAR(100) NOT NULL UNIQUE,
    DepartamentoID INT NOT NULL,
    FOREIGN KEY (DepartamentoID) REFERENCES Departamento(DepartamentoID)
);

CREATE TABLE GradeCurricular (
    GradeCurricularID INT AUTO_INCREMENT PRIMARY KEY,
    DisciplinaID INT NOT NULL,
    FOREIGN KEY (DisciplinaID) REFERENCES Disciplina(DisciplinaID),
    CursoID INT NOT NULL,
    FOREIGN KEY (CursoID) REFERENCES Curso(CursoID)
);

CREATE TABLE Prerequisito (
    DisciplinaID INT NOT NULL,
    PreRequisitoID INT NOT NULL,
    PRIMARY KEY (DisciplinaID, PreRequisitoID), 
    FOREIGN KEY (DisciplinaID) REFERENCES Disciplina(DisciplinaID),
    FOREIGN KEY (PreRequisitoID) REFERENCES Disciplina(DisciplinaID)
);

CREATE TABLE Matricula (
    MatriculaID INT AUTO_INCREMENT PRIMARY KEY,
    AlunoID INT NOT NULL,
    FOREIGN KEY (AlunoID) REFERENCES Aluno(AlunoID),
    CursoID INT NOT NULL,
    FOREIGN KEY (CursoID) REFERENCES Curso(CursoID),
    DataMatricula DATE NOT NULL,
    MatriculaAtiva BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE Nota (
    NotaID INT AUTO_INCREMENT PRIMARY KEY,
    NotaValor DECIMAL(4, 2) NOT NULL,
    AlunoID INT NOT NULL,
    FOREIGN KEY (AlunoID) REFERENCES Aluno(AlunoID),
    DisciplinaID INT NOT NULL,
    FOREIGN KEY (DisciplinaID) REFERENCES Disciplina(DisciplinaID)
);

CREATE TABLE Turma (
    TurmaID INT AUTO_INCREMENT PRIMARY KEY,
    Semestre VARCHAR(10) NOT NULL,
    DisciplinaID INT NOT NULL,
    FOREIGN KEY (DisciplinaID) REFERENCES Disciplina(DisciplinaID),
    SalaDeAulaID INT NOT NULL,
    FOREIGN KEY (SalaDeAulaID) REFERENCES SalaDeAula(SalaDeAulaID),
    ProfessorID INT NOT NULL,
    FOREIGN KEY (ProfessorID) REFERENCES Professor(ProfessorID),
    HorarioID INT NOT NULL,
    FOREIGN KEY (HorarioID) REFERENCES Horario(HorarioID)
);
```

### Configuração do appsettings.json
Configure a string de conexão e os parâmetros do JWT. Substitua os placeholders pelos seus valores secretos.

```bash
{
  "Logging": { /* ... */ },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=universidadedb;User Id=root;Password=SUA_SENHA_MYSQL;"
  },
  "Jwt": {
    "Key": "SUA_CHAVE_SECRETA_MUITO_LONGA_E_COMPLEXA_COM_PELO_MENOS_32_CARACTERES", 
    "Issuer": "UniversidadeAPI",
    "Audience": "UniversidadeAppUsuarios"
  }
}

```

### Setup Rápido: Populando o Banco (Massa de Dados)
Após criar as tabelas com o script acima, execute o script SQL abaixo uma única vez para popular (fazer o seeding) do banco de dados com dados de teste.

```bash

-- ===============================================
-- SCRIPT DE POPULAÇÃO (MASSA DE DADOS) - CORRIGIDO
-- ===============================================
START TRANSACTION;

-- NÍVEL 1: Entidades sem dependências
-- Usuário Admin (Login: "admin", Senha: "admin123")
INSERT INTO Usuario (Login, SenhaHash, Role)
VALUES ('admin', '$2a$11$uS.If0m/B.3G.P5tB.WJ5uYw.AUv6C.FkF.E73wK/yC.Nl51.J/iO', 'Admin');

-- Departamentos
INSERT INTO Departamento (DepartamentoNome) VALUES ('Exatas');
SET @deptoExatasId := LAST_INSERT_ID(); 
INSERT INTO Departamento (DepartamentoNome) VALUES ('Humanas');
SET @deptoHumanasId := LAST_INSERT_ID();

-- Professores
INSERT INTO Professor (ProfessorNome, DataNascimento, RG, CPF, Formacao)
VALUES ('Albert Einstein', '1879-03-14', '1234567-8', '11111111111', 'Doutorado em Física');
SET @profEinsteinId := LAST_INSERT_ID();
INSERT INTO Professor (ProfessorNome, DataNascimento, RG, CPF, Formacao)
VALUES ('Simone de Beauvoir', '1908-01-09', '8765432-1', '22222222222', 'Doutorado em Filosofia');
SET @profSimoneId := LAST_INSERT_ID();

-- Alunos
INSERT INTO Aluno (AlunoNome, DataNascimento, RG, CPF)
VALUES ('Marie Curie', '1867-11-07', '9876543-2', '33333333333');
SET @alunoMarieId := LAST_INSERT_ID();
INSERT INTO Aluno (AlunoNome, DataNascimento, RG, CPF)
VALUES ('Santos Dumont', '1873-07-20', '2345678-9', '44444444444');
SET @alunoSantosId := LAST_INSERT_ID();

-- Usuários para Alunos e Professores (Login = CPF, Senha = "nome123")
INSERT INTO Usuario (Login, SenhaHash, Role)
VALUES ('11111111111', '$2a$11$0K7pI.gKqV.x/.BylQ.5A.wG1e.jB6A1O8G.gO/E.Vj.1G33K.P.6', 'Professor'); -- einstein123
INSERT INTO Usuario (Login, SenhaHash, Role)
VALUES ('22222222222', '$2a$11$F7T5aGjR.kSgJzJkY3.vve1T4.yG./iC36o/3p/O.PRa3g11kSjG2', 'Professor'); -- simone123
INSERT INTO Usuario (Login, SenhaHash, Role)
VALUES ('33333333333', '$2a$11$G..I/31/5uKzG8fKjF.fPucNMG9j6/5iDBmy.d0/bN.t8a/Ebsg9S', 'Aluno'); -- curie123
INSERT INTO Usuario (Login, SenhaHash, Role)
VALUES ('44444444444', '$2a$11$a4Bq5G.13.N48.R/O3.2k./n8n1N1P2.48n8/N.k3B.7c.3e.Z/9O', 'Aluno'); -- santos123

-- Salas de Aula
INSERT INTO SalaDeAula (Capacidade, NumeroSala, PredioNome)
VALUES (50, 101, 'Bloco A');
SET @sala101Id := LAST_INSERT_ID();
INSERT INTO SalaDeAula (Capacidade, NumeroSala, PredioNome)
VALUES (30, 202, 'Bloco B');
SET @sala202Id := LAST_INSERT_ID();

-- Disciplinas
INSERT INTO Disciplina (NomeDisciplina) VALUES ('Cálculo I');
SET @discCalc1Id := LAST_INSERT_ID();
INSERT INTO Disciplina (NomeDisciplina) VALUES ('Cálculo II');
SET @discCalc2Id := LAST_INSERT_ID();
INSERT INTO Disciplina (NomeDisciplina) VALUES ('Filosofia Contemporânea');
SET @discFilosofiaId := LAST_INSERT_ID();

-- Horarios
INSERT INTO Horario (DiaSemana, HoraInicio, HoraFim)
VALUES ('Segunda', '08:00:00', '10:00:00');
SET @horarioCalc1Id := LAST_INSERT_ID();
INSERT INTO Horario (DiaSemana, HoraInicio, HoraFim)
VALUES ('Terça', '10:00:00', '12:00:00'); 
SET @horarioFiloId := LAST_INSERT_ID();

-- NÍVEL 2: Dependem do Nível 1
-- Cursos
INSERT INTO Curso (NomeCurso, DepartamentoID)
VALUES ('Física', @deptoExatasId);
SET @cursoFisicaId := LAST_INSERT_ID();
INSERT INTO Curso (NomeCurso, DepartamentoID)
VALUES ('Filosofia', @deptoHumanasId);
SET @cursoFilosofiaId := LAST_INSERT_ID();

-- Prerequisitos (Calc II depende de Calc I)
INSERT INTO Prerequisito (DisciplinaID, PreRequisitoID)
VALUES (@discCalc2Id, @discCalc1Id);

-- Notas
INSERT INTO Nota (NotaValor, AlunoID, DisciplinaID)
VALUES (9.5, @alunoMarieId, @discCalc1Id);
INSERT INTO Nota (NotaValor, AlunoID, DisciplinaID)
VALUES (8.0, @alunoSantosId, @discCalc1Id);
        
-- NÍVEL 3: Dependem do Nível 2
-- GradeCurriculares
INSERT INTO GradeCurricular (DisciplinaID, CursoID)
VALUES (@discCalc1Id, @cursoFisicaId);
INSERT INTO GradeCurricular (DisciplinaID, CursoID)
VALUES (@discCalc2Id, @cursoFisicaId);
INSERT INTO GradeCurricular (DisciplinaID, CursoID)
VALUES (@discFilosofiaId, @cursoFilosofiaId);

-- Matriculas
INSERT INTO Matricula (AlunoID, CursoID, DataMatricula, MatriculaAtiva)
VALUES (@alunoMarieId, @cursoFisicaId, CURDATE(), 1); 
INSERT INTO Matricula (AlunoID, CursoID, DataMatricula, MatriculaAtiva)
VALUES (@alunoSantosId, @cursoFisicaId, CURDATE(), 1);

-- Turmas
INSERT INTO Turma (Semestre, DisciplinaID, SalaDeAulaID, ProfessorID, HorarioID)
VALUES ('2025.2', @discCalc1Id, @sala101Id, @profEinsteinId, @horarioCalc1Id);
INSERT INTO Turma (Semestre, DisciplinaID, SalaDeAulaID, ProfessorID, HorarioID)
VALUES ('2025.2', @discFilosofiaId, @sala202Id, @profSimoneId, @horarioFiloId);

COMMIT;

```

### Como Rodar e Testar a API
Abra o projeto no Visual Studio.

Pressione F5 (Debug) ou Ctrl+F5 (Sem Debug).

A interface do Swagger UI será aberta.

Fluxo de Teste:
Acesse POST /api/Auth/login.

Use as credenciais do admin do script de seeding (Login: admin, Senha: admin123) para obter o Token JWT.

Use o botão Authorize no Swagger para colar o Token (formato Bearer [Token]).

Teste as rotas protegidas (ex: GET /api/Alunos, POST /api/Cursos, etc.).

### Arquitetura da API e Endpoints
A API está organizada em 13 controladores principais, todos protegidos por autenticação JWT. Operações de criação, atualização e exclusão são restritas a Roles específicas (principalmente Admin).

| Controlador | Rota | Descrição |
| :--- | :--- | :--- |
| **`Auth`** | `/api/Auth` | Autenticação (Login/Register) |
| **`Alunos`** | `/api/Alunos` | CRUD de Alunos |
| **`Cursos`** | `/api/Cursos` | CRUD de Cursos |
| **`Departamentos`** | `/api/Departamentos` | CRUD de Departamentos |
| **`Disciplinas`** | `/api/Disciplinas` | CRUD de Disciplinas |
| **`GradeCurricular`** | `/api/GradeCurricular` | Relação Curso <-> Disciplina |
| **`Horarios`** | `/api/Horarios` | CRUD de Horários |
| **`Matriculas`** | `/api/Matriculas` | Gestão de Matrículas |
| **`Notas`** | `/api/Notas` | Gestão de Notas |
| **`Prerequisitos`** | `/api/Prerequisitos` | Relação Disciplina <-> Disciplina |
| **`Professores`** | `/api/Professores` | CRUD de Professores |
| **`SalasDeAula`** | `/api/SalasDeAula` | CRUD de Salas |
| **`Turmas`** | `/api/Turmas` | Gestão de Turmas |

---

### Roadmap e Próximas Etapas

Com a API RESTful de backend agora completa, o foco principal muda para a criação do cliente (front-end) e a otimização da API.

#### Próxima Etapa Principal

* **Implementar o Front-End**: Desenvolver a aplicação do cliente (Single Page Application - SPA) usando **Angular** e **TypeScript** para consumir esta API.

#### Sugestões de Melhoria para a API

* **Refatoração (AutoMapper)**: Substituir o mapeamento manual (Entidade <-> DTO) nos controladores pela biblioteca `AutoMapper`. Isso limpará os controladores e centralizará as regras de mapeamento.
* **Otimização (DTOs Enriquecidos)**: Modificar os repositórios (ex: `ITurmaRepository`) para usar `JOINs` em SQL e retornar DTOs de resposta (`TurmaResponseDto`) que contenham nomes (ex: `NomeDisciplina`, `NomeProfessor`) em vez de apenas IDs.
* **Funcionalidade (Paginação)**: Adicionar suporte para paginação global (`?page=...&pageSize=...`) nos endpoints `GET` (Todos) para lidar com grandes volumes de dados de forma eficiente.
* **Funcionalidade (Gestão de Usuários)**: Implementar um endpoint de "promoção" de Roles (ex: `PUT /api/Auth/promote/{id}`) para que um `Admin` possa alterar a `Role` de outros usuários (`Professor`, `Aluno`).
