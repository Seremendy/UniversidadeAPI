# 🎓 UniversidadeAPI

API RESTful construída em **ASP.NET Core 8.0** para o gerenciamento de dados acadêmicos de uma universidade.  
O sistema cobre todo o ciclo de vida acadêmico, incluindo **alunos, professores, cursos, matrículas, turmas e lançamento de notas**.

O projeto segue princípios de **Arquitetura Limpa**, utiliza o **padrão Repository** com **Dapper** para alta performance no acesso a dados e **JWT com controle de Roles (RBAC)** para segurança robusta.

---

## 🚀 Features Principais

- **Autenticação & Segurança**
  - Login com **JWT (JSON Web Tokens)**
  - Senhas criptografadas com **BCrypt**
- **Controle de Acesso (RBAC)**
  - Roles: `Admin`, `Professor`, `Aluno`
- **Alta Performance**
  - Uso de **Dapper (Micro-ORM)** com SQL otimizado
- **Integridade de Dados**
  - Validações de regras de negócio (pré-requisitos, horários, vínculos)
- **CRUD Completo**
  - Gerenciamento de **13 entidades relacionais**
- **Documentação**
  - Swagger (OpenAPI) integrado

---

## 🛠️ Tecnologias Utilizadas (Tech Stack)

| Categoria           | Tecnologia                     | Detalhes                                      |
|---------------------|--------------------------------|-----------------------------------------------|
| Backend             | ASP.NET Core 8.0               | Web API (LTS)                                 |
| Linguagem           | C# 12                          | Recursos modernos da linguagem                |
| Banco de Dados      | MySQL 8.0+                     | Relacional                                    |
| Acesso a Dados      | Dapper                         | Micro-ORM de alta performance                 |
| Segurança           | JWT & BCrypt                   | Autenticação Stateless e Hashing              |
| Documentação        | Swagger (OpenAPI)              | Interface interativa de testes                |

---

## ⚙️ Configuração e Setup

### Pré-requisitos

- .NET SDK **8.0**
- MySQL Server **8.0+**

---

### Instalação de Dependências

Execute na raiz do projeto:

```bash
dotnet add package Dapper
dotnet add package MySqlConnector
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package BCrypt.Net-Next
```
### Configuração do Banco de Dados

Execute o script abaixo no MySQL (Workbench, DBeaver, etc.):
```
-- ===============================================
-- 1. CRIAÇÃO DO SCHEMA (DDL)
-- ===============================================
CREATE DATABASE IF NOT EXISTS universidadedb;
USE universidadedb;

-- Entidades Independentes
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

-- Entidades Dependentes
CREATE TABLE Curso (
    CursoID INT AUTO_INCREMENT PRIMARY KEY,
    NomeCurso VARCHAR(100) NOT NULL UNIQUE,
    DepartamentoID INT NOT NULL,
    FOREIGN KEY (DepartamentoID) REFERENCES Departamento(DepartamentoID)
);

CREATE TABLE GradeCurricular (
    GradeCurricularID INT AUTO_INCREMENT PRIMARY KEY,
    DisciplinaID INT NOT NULL,
    CursoID INT NOT NULL,
    FOREIGN KEY (DisciplinaID) REFERENCES Disciplina(DisciplinaID),
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
    CursoID INT NOT NULL,
    DataMatricula DATE NOT NULL,
    MatriculaAtiva BOOLEAN NOT NULL DEFAULT TRUE,
    FOREIGN KEY (AlunoID) REFERENCES Aluno(AlunoID),
    FOREIGN KEY (CursoID) REFERENCES Curso(CursoID)
);

CREATE TABLE Nota (
    NotaID INT AUTO_INCREMENT PRIMARY KEY,
    NotaValor DECIMAL(4,2) NOT NULL,
    AlunoID INT NOT NULL,
    DisciplinaID INT NOT NULL,
    FOREIGN KEY (AlunoID) REFERENCES Aluno(AlunoID),
    FOREIGN KEY (DisciplinaID) REFERENCES Disciplina(DisciplinaID)
);

CREATE TABLE Turma (
    TurmaID INT AUTO_INCREMENT PRIMARY KEY,
    Semestre VARCHAR(10) NOT NULL,
    DisciplinaID INT NOT NULL,
    SalaDeAulaID INT NOT NULL,
    ProfessorID INT NOT NULL,
    HorarioID INT NOT NULL,
    FOREIGN KEY (DisciplinaID) REFERENCES Disciplina(DisciplinaID),
    FOREIGN KEY (SalaDeAulaID) REFERENCES SalaDeAula(SalaDeAulaID),
    FOREIGN KEY (ProfessorID) REFERENCES Professor(ProfessorID),
    FOREIGN KEY (HorarioID) REFERENCES Horario(HorarioID)
);
```

### Configuração do appsettings.json
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=universidadedb;Uid=root;Pwd=SUA_SENHA_MYSQL;"
  },
  "Jwt": {
    "Key": "SUA_CHAVE_SECRETA_MUITO_LONGA_E_COMPLEXA_COM_PELO_MENOS_32_CARACTERES",
    "Issuer": "UniversidadeAPI",
    "Audience": "UniversidadeAppUsuarios"
  }
}
```

### Endpoints da API

A API está documentada via Swagger:
| Recurso            | Descrição               | Permissões        |
| ------------------ | ----------------------- | ----------------- |
| `/api/Auth`        | Login / Registro        | Público / Admin   |
| `/api/Alunos`      | Gestão de alunos        | Admin             |
| `/api/Professores` | Gestão de professores   | Admin             |
| `/api/Cursos`      | Gestão de cursos        | Admin             |
| `/api/Disciplinas` | Catálogo de disciplinas | Admin             |
| `/api/Matriculas`  | Vínculo Aluno-Curso     | Admin             |
| `/api/Notas`       | Lançamento de notas     | Professor / Admin |
| `/api/Turmas`      | Formação de turmas      | Admin             |
| `/api/Horarios`    | Grade horária           | Admin             |
| `/api/SalasDeAula` | Gestão de salas         | Admin             |

### Roadmap

 [x] Backend completo com Dapper e JWT.

 [x] Modelagem relacional e scripts de seeding.

 [] Frontend em Angular (em progresso).

 [] Refatoração com AutoMapper e Paginação.
