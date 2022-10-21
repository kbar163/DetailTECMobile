
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
        public Customer GetCustomer(string userName)
        {
            try
            {
                var customer = new Customer();

                var customerQuery = _database.Query<Customer>($@"SELECT CLIENTE.CEDULA_CLIENTE, CLIENTE.NOMBRE, CLIENTE.PRIMER_APELLIDO,
                    CLIENTE.SEGUNDO_APELLIDO, CLIENTE.CORREO_CLIENTE, CLIENTE.USUARIO, CLIENTE.PASSWORD_CLIENTE,
                    CLIENTE.PUNTOS_ACUM , CLIENTE.PUNTOS_OBT , CLIENTE.PUNTOS_REDIM
                    FROM CLIENTE WHERE CLIENTE.USUARIO = ?", userName);
                

                if(customerQuery.Count != 0)
                {
                    customer = customerQuery[0];
                    var addressQuery = _database.Query<Address>($@"SELECT CLIENTE_DIRECCION.PROVINCIA,
                    CLIENTE_DIRECCION.CANTON, CLIENTE_DIRECCION.DISTRITO
                    FROM CLIENTE_DIRECCION WHERE CLIENTE_DIRECCION.CEDULA_CLIENTE = ?", customer.cedula_cliente);
                    customer.direcciones = addressQuery;

                    var phoneQuery = _database.Query<Phone>($@"SELECT CLIENTE_TELEFONO.TELEFONO
                    FROM CLIENTE_TELEFONO WHERE CLIENTE_TELEFONO.CEDULA_CLIENTE = ?", customer.cedula_cliente);
                    customer.telefonos = phoneQuery;
                   
                }
                return customer;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            
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
            AddUsers();
        }

        public void CreateAddressTable()
        {
            _database.Execute(@"CREATE TABLE IF NOT EXISTS CLIENTE_DIRECCION 
                (
                CEDULA_CLIENTE VARCHAR(9) NOT NULL ,
                PROVINCIA VARCHAR(50) NOT NULL ,
                CANTON VARCHAR(50) NOT NULL ,
                DISTRITO VARCHAR(50) NOT NULL ,
                PRIMARY KEY (CEDULA_CLIENTE, PROVINCIA, CANTON, DISTRITO),
                FOREIGN KEY (CEDULA_CLIENTE) REFERENCES CLIENTE(CEDULA_CLIENTE)
                );");
            AddUserAdress();
        }

        public void CreatePhoneTable()
        {
            _database.Execute(@"CREATE TABLE IF NOT EXISTS CLIENTE_TELEFONO 
                (
                CEDULA_CLIENTE VARCHAR(9) NOT NULL ,
                TELEFONO VARCHAR(20) NOT NULL,
                FOREIGN KEY (CEDULA_CLIENTE) REFERENCES CLIENTE(CEDULA_CLIENTE)
                );");
            AddUserPhones();
        }
        #endregion

        #region Population
        private void AddUsers()
        {
            try
            {
                _database.Execute(@"INSERT INTO CLIENTE
                    VALUES 
                    ('409837345','Erick', 'Barrantes', 'Cerdas', 'correo01@gmail.com', 'user01', 'password01',9, 15, 6),
                    ('709837345','Manuel', 'Brenes', 'Salazar', 'correo02@gmail.com', 'user02', 'password02',0, 5, 5),
                    ('609837345','Sofia', 'Ramirez', 'Gomez', 'correo03@gmail.com', 'user03', 'password03',5, 5, 0),
                    ('209837345','Veronica', 'Sandoval', 'Sanchez', 'correo04@gmail.com', 'user04', 'password04',12, 17, 5);");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        private void AddUserPhones()
        {
            try
            {
            _database.Execute(@"INSERT INTO CLIENTE_TELEFONO
                VALUES
                ('409837345','89881111'),
                ('409837345','89882222'),
                ('409837345','89883333'),
                ('709837345','88871111'),
                ('609837345','87861111'),
                ('609837345','87862222'),
                ('209837345','86851111');");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        private void AddUserAdress()
        {
            try
            {
                _database.Execute(@"INSERT INTO CLIENTE_DIRECCION
                    VALUES
                    ('409837345','San José','Montes De Oca','San Pedro'),
                    ('409837345','Cartago','Turrialba','Turrialba'),
                    ('709837345','Alajuela','Palmares','Palmares'),
                    ('609837345','Heredia','San Isidro','San Francisco'),
                    ('209837345','Puntarenas','Coto Brus','San Vito'),
                    ('209837345','Puntarenas','Coto Brus','Sabalito');");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
        #endregion


    }
}
