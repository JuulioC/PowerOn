DROP DATABASE IF EXISTS POWERON;
CREATE DATABASE POWERON;
USE POWERON;

/*------------------- TABELAS -------------------*/
CREATE TABLE Marca (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Cod VARCHAR(255),
    Nome VARCHAR(255)
);

CREATE TABLE Familia (	
	Id INT AUTO_INCREMENT PRIMARY KEY,
	Cod VARCHAR(255),
	Nome VARCHAR(255),
	IdMarca INT
);

CREATE TABLE Modelo (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Cod VARCHAR(255),
    Nome VARCHAR(255),
    AnoFabricacao VARCHAR(255),
    AnoModelo VARCHAR(255),
    IdFamilia INT
);

CREATE TABLE Revisao (	
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(255),
    IdMarca INT,
    IdModelo INT,
    Km VARCHAR(255),
    Motor VARCHAR(255),
    Ativo INT
);

-- Tabela Produto: Sem alterações, continua sendo a fonte dos produtos.
CREATE TABLE Produto (		
	Id INT AUTO_INCREMENT PRIMARY KEY,
	Nome VARCHAR(255),
	Tipo INT,
	CodSistema VARCHAR(255),
	Valor FLOAT
);

-- Tabela Pacote: AJUSTADA para não ter mais vínculo direto com produtos.
CREATE TABLE Pacote (
	Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(255),
    Descricao TEXT NULL -- Campo adicionado para descrever o pacote.
);

-- NOVA TABELA: Tabela de ligação para a relação Muitos-para-Muitos.
CREATE TABLE PacoteProduto (
    IdPacote INT NOT NULL,
    IdProduto INT NOT NULL,
    Quantidade INT NOT NULL DEFAULT 1, -- Quantidade do produto dentro do pacote.
    PRIMARY KEY (IdPacote, IdProduto), -- Garante que um produto não se repita no mesmo pacote.
    FOREIGN KEY (IdPacote) REFERENCES Pacote(Id) ON DELETE CASCADE,
    FOREIGN KEY (IdProduto) REFERENCES Produto(Id) ON DELETE CASCADE
);

CREATE TABLE Sistemas (
	Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(255)
);

CREATE TABLE Empresa (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(255),
    CNPJ VARCHAR(255),
    IdSistema INT
);

CREATE TABLE Usuario (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(255),
    Email VARCHAR(255),
    Senha VARCHAR(255),
    Perfil INT,
    EmpresaId INT,
    CodigoSistema VARCHAR(255),
    ImgPerfil LONGBLOB,
    Ativo INT,
    UltimoAcesso DATETIME,
    FOREIGN KEY (EmpresaId) REFERENCES Empresa(Id)
);

CREATE TABLE Agenda (
    Id INT AUTO_INCREMENT PRIMARY KEY,
	Data_Agendamento DATETIME,
    Cliente VARCHAR(255),
    Placa VARCHAR(255),
    IdConsultor VARCHAR(255),
    IdModelo VARCHAR(255),
    Observacao VARCHAR(255), -- CORRIGIDO: Observacai -> Observacao
    OS VARCHAR(255),
    Situacao VARCHAR(255)
);

CREATE TABLE Cliente (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(255),
    TipoPessoa VARCHAR(255),
    Sexo VARCHAR(255),
    CPFCNPJ VARCHAR(255),
    RG VARCHAR(255),
    Email VARCHAR(255),
    PermiteSMS INT,
    PermiteEMAIL INT,
    PermiteWHATSAPP INT
);

CREATE TABLE ClienteEndereco (
	Id INT AUTO_INCREMENT PRIMARY KEY,
    Cliente INT,
    Tipo VARCHAR(255),
    Logradouro VARCHAR(255),
    Numero VARCHAR(255),
    Complemento VARCHAR(255),
    Bairro VARCHAR(255),
    Cidade VARCHAR(255),
    Estado VARCHAR(255),
    CEP VARCHAR(255)
);

CREATE TABLE ClienteTelefone (
	Id INT AUTO_INCREMENT PRIMARY KEY,
    Cliente INT,
    Tipo VARCHAR(255),
    DDD VARCHAR(255),
    Telefone VARCHAR(255)
);

-- CORRIGIDO: Viculo -> Veiculo
CREATE TABLE Veiculo (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Placa VARCHAR(255),
    Chassi VARCHAR(255),
	Cliente INT,	
    Marca VARCHAR(255),
    Familia VARCHAR(255),
    Modelo VARCHAR(255),
    Motor VARCHAR(255),
    Km VARCHAR(255)
);

CREATE TABLE logApp(
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Caminho LONGTEXT,
    Mensagem LONGTEXT,
    Usuario LONGTEXT,
    DataErro DATETIME	
);

/*------------------- DADOS FIXOS -------------------*/
INSERT INTO Empresa (Nome, CNPJ, IdSistema)
VALUES('MAXMOBILITY','08.678.729/0001-23',0);

-- Note que o ID da empresa será 1
INSERT INTO Usuario (Nome, Email, Senha, Perfil, Ativo, EmpresaId)
VALUES('mastermax', 'admin@emai.com', '78UeJiyW9pRK4NCxXN5zTg==', 0, 1, 1);
