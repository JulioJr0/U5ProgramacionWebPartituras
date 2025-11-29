-- MySQL dump 10.13  Distrib 8.0.28, for Win64 (x86_64)
--
CREATE DATABASE IF NOT EXISTS `partituras`;
USE `partituras`;
-- Host: localhost    Database: partituras
-- ------------------------------------------------------
-- Server version	8.0.28

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `compositor`
--

DROP TABLE IF EXISTS `compositor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `compositor` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(100) NOT NULL,
  `Nacionalidad` varchar(45) DEFAULT NULL,
  `Biografia` text,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `compositor`
--

LOCK TABLES `compositor` WRITE;
/*!40000 ALTER TABLE `compositor` DISABLE KEYS */;
INSERT INTO `compositor` VALUES (1,'Wolfgang Amadeus Mozart','Austríaca','Compositor prolífico del Clasicismo, reconocido por su vasto repertorio de óperas y sinfonías.'),(2,'Scott Joplin','Estadounidense','Apodado el \"Rey del Ragtime\", fue una figura clave en el desarrollo del jazz temprano.'),(3,'John Williams','Estadounidense','Uno de los compositores más famosos de bandas sonoras de cine, conocido por Star Wars y Harry Potter.'),(4,'Johann Sebastian Bach','Alemana','Compositor del período Barroco, maestro del contrapunto y la fuga.'),(5,'Queen (Banda)','Británica','Banda de rock conocida por su diversidad musical y complejas armonías vocales.');
/*!40000 ALTER TABLE `compositor` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `genero`
--

DROP TABLE IF EXISTS `genero`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `genero` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(45) NOT NULL,
  `Descripcion` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `genero`
--

LOCK TABLES `genero` WRITE;
/*!40000 ALTER TABLE `genero` DISABLE KEYS */;
INSERT INTO `genero` VALUES (1,'Clásico','Obras de música artística de Occidente, de los períodos Barroco, Clásico y Romántico.'),(2,'Jazz','Música caracterizada por la improvisación, ritmos sincopados y un fuerte énfasis en la interpretación.'),(3,'Pop/Rock','Partituras de canciones populares, tanto pop como rock.'),(4,'Banda Sonora','Música escrita específicamente para acompañar una película o programa de televisión.'),(5,'Música de Cámara','Obras compuestas para un pequeño grupo de instrumentos, tradicionalmente sin director.');
/*!40000 ALTER TABLE `genero` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `partitura`
--

DROP TABLE IF EXISTS `partitura`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `partitura` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Titulo` varchar(255) NOT NULL,
  `Instrumentacion` varchar(100) DEFAULT NULL,
  `Dificultad` varchar(45) DEFAULT NULL COMMENT 'Ej: Fácil, Intermedio, Avanzado',
  `Descripcion` text,
  `IdGenero` int NOT NULL,
  `IdCompositor` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_Partitura_Genero` (`IdGenero`),
  KEY `fk_Partitura_Compositor` (`IdCompositor`),
  CONSTRAINT `fk_Partitura_Compositor` FOREIGN KEY (`IdCompositor`) REFERENCES `compositor` (`Id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `fk_Partitura_Genero` FOREIGN KEY (`IdGenero`) REFERENCES `genero` (`Id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `partitura`
--

LOCK TABLES `partitura` WRITE;
/*!40000 ALTER TABLE `partitura` DISABLE KEYS */;
INSERT INTO `partitura` VALUES (1,'Sonata para Piano n.º 16 en Do Mayor (K. 545)','Piano Solo','Intermedio','Una de las sonatas más famosas de Mozart, conocida por su claridad estructural y melodías elegantes.',1,1),(2,'Maple Leaf Rag','Piano Solo','Intermedio','La pieza que definió el género Ragtime, con un ritmo contagioso y sincopado.',2,2),(3,'Hedwig\'s Theme','Orquesta / Piano','Fácil-Intermedio','Tema principal de la saga Harry Potter, evocador y mágico.',4,3),(4,'Fuga en Sol menor (BWV 578)','Órgano','Avanzado','Una de las fugas más conocidas de Bach, perfecta para demostrar la complejidad contrapuntística.',1,4),(5,'Bohemian Rhapsody','Piano/Vocal','Avanzado','Épica de rock con múltiples secciones que cambian drásticamente de estilo y ritmo.',3,5);
/*!40000 ALTER TABLE `partitura` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-11-05 21:31:40
USE `partituras`;