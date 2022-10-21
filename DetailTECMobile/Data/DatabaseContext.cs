
using DetailTECMobile.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DetailTECMobile.Data
{   
    //Contexto de la base de datos empotrada creada usando SQLite, se manejan las operaciones CRUD de los distintos elementos de la aplicacion
    //La clase tambien se crea de la creacion de la base de datos y la carga de datos iniciales la primera vez que se ejecuta la aplicacion
    //en el dispositivo.
    public class DatabaseContext
    {


        readonly SQLiteConnection _database;
        
        public DatabaseContext(string dbPath)
        {
            _database = new SQLiteConnection(dbPath);
          
        }

        #region CRUD CLIENTES
        public List<Customer> GetCustomer(string userName)
        {
            var query = _database.Query<Customer>($@"SELECT CLIENTE.CEDULA_CLIENTE, CLIENTE.NOMBRE, CLIENTE.PRIMER_APELLIDO,
            CLIENTE.SEGUNDO_APELLIDO, CLIENTE.CORREO_CLIENTE, CLIENTE.USUARIO, CLIENTE.PASSWORD_CLIENTE,
            CLIENTE.PUNTOS_ACUM , CLIENTE.PUNTOS_OBT , CLIENTE.PUNTOS_REDIM
            FROM CLIENTE WHERE CLIENTE.USUARIO = ?", userName);
            return query;
        }
        #endregion

        #region Creation
        public void CreateCustomerTable()
        {
           _database.Execute(@"CREATE TABLE IF NOT EXISTS CLIENTE 
           (
           CEDULA_CLIENTE VARCHAR(9) NOT NULL PRIMARY KEY,
           NOMBRE VARCHAR(20) NOT NULL,
           PRIMER_APELLIDO VARCHAR(20) NOT NULL,
           SEGUNDO_APELLIDO VARCHAR(20) NOT NULL,
           CORREO_CLIENTE VARCHAR(50) NOT NULL,
           USUARIO VARCHAR(20) NOT NULL,
           PASSWORD_CLIENTE VARCHAR(20) NOT NULL,
           PUNTOS_ACUM INT,
           PUNTOS_OBT INT,
           PUNTOS_REDIM INT,
           CONSTRAINT UNIQUE_LOGIN UNIQUE(CORREO_CLIENTE, USUARIO)
           );");  
        }
        #endregion

        #region Population
        private void AddUsers()
        {
            _database.Execute(@"INSERT INTO CLIENTE
            VALUES 
            ('409837345','Erick', 'Barrantes', 'Cerdas', 'correo01@gmail.com', 'user01', 'password01',9, 15, 6),
            ('709837345','Manuel', 'Brenes', 'Salazar', 'correo02@gmail.com', 'user02', 'password02',0, 5, 5),
            ('609837345','Sofia', 'Ramirez', 'Gomez', 'correo03@gmail.com', 'user03', 'password03',5, 5, 0),
            ('209837345','Veronica', 'Sandoval', 'Sanchez', 'correo04@gmail.com', 'user04', 'password04',12, 17, 5);");
        }
        #endregion


    }
}
