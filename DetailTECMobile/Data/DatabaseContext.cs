
using DetailTECMobile.Models;
using DetailTECMobile.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

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
        public async Task<Customer> GetCustomer(string userName)
        {
            try
            {
                var customer = new Customer();

                var customerQuery = _database.Query<Customer>($@"SELECT CLIENTE.CEDULA_CLIENTE, CLIENTE.NOMBRE, CLIENTE.PRIMER_APELLIDO,
                    CLIENTE.SEGUNDO_APELLIDO, CLIENTE.CORREO_CLIENTE, CLIENTE.USUARIO, CLIENTE.PASSWORD_CLIENTE,
                    CLIENTE.PUNTOS_ACUM , CLIENTE.PUNTOS_OBT , CLIENTE.PUNTOS_REDIM
                    FROM CLIENTE WHERE CLIENTE.USUARIO = ?", userName);


                if (customerQuery.Count != 0)
                {
                    customer = customerQuery[0];
                    var addressQuery = _database.Query<Address>($@"SELECT CLIENTE_DIRECCION.PROVINCIA,
                    CLIENTE_DIRECCION.CANTON, CLIENTE_DIRECCION.DISTRITO
                    FROM CLIENTE_DIRECCION WHERE CLIENTE_DIRECCION.CEDULA_CLIENTE = ?", customer.cedula_cliente);
                    customer.direcciones = addressQuery;

                    var phoneQuery = _database.Query<Phone>($@"SELECT CLIENTE_TELEFONO.TELEFONO
                    FROM CLIENTE_TELEFONO WHERE CLIENTE_TELEFONO.CEDULA_CLIENTE = ?", customer.cedula_cliente);

                    customer.telefonos = new List<string>();
                    foreach (Phone element in phoneQuery)
                    {
                        customer.telefonos.Add(element.telefono);
                    }

                }
                return customer;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }

        public async Task UpdateCustomer(Customer updatedCustomer)
        {
            try
            {
                var customerQuery = _database.Execute(@"UPDATE CLIENTE
                SET CEDULA_CLIENTE = ?,
                NOMBRE = ?,
                PRIMER_APELLIDO = ?,
                SEGUNDO_APELLIDO = ?,
                CORREO_CLIENTE = ?,
                USUARIO = ?,
                PASSWORD_CLIENTE = ?,
                PUNTOS_ACUM = ?,
                PUNTOS_OBT = ?,
                PUNTOS_REDIM = ?
                WHERE CEDULA_CLIENTE = ?",
                updatedCustomer.cedula_cliente,
                updatedCustomer.nombre,
                updatedCustomer.primer_apellido,
                updatedCustomer.segundo_apellido,
                updatedCustomer.correo_cliente,
                updatedCustomer.usuario,
                updatedCustomer.password_cliente,
                updatedCustomer.puntos_acum,
                updatedCustomer.puntos_obt,
                updatedCustomer.puntos_redim,
                updatedCustomer.cedula_cliente);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task SyncCustomers(MultivalueCustomer customers)
        {
            try
            {

                foreach (Customer customer in customers.clientes)
                {
                    var check = await GetCustomer(customer.usuario);
                    if (check != null && check.cedula_cliente == null)
                    {
                        _database.Execute(@"INSERT INTO CLIENTE
                         VALUES 
                         (? , ? , ? , ? , ? , ? , ? , ?, ? , ?)",
                        customer.cedula_cliente, customer.nombre,
                        customer.primer_apellido, customer.segundo_apellido,
                        customer.correo_cliente, customer.usuario, customer.password_cliente,
                        customer.puntos_acum, customer.puntos_obt, customer.puntos_redim);
                        foreach (Address element in customer.direcciones)
                        {
                            _database.Execute(@"INSERT INTO CLIENTE_DIRECCION
                            VALUES
                            (?,?,?,?)", customer.cedula_cliente, element.provincia, element.canton, element.distrito);
                        }
                        foreach (string phone in customer.telefonos)
                        {
                            _database.Execute(@"INSERT INTO CLIENTE_TELEFONO
                            VALUES
                            (?,?)", customer.cedula_cliente, phone);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task<MultivalueWash> syncWashTypes()
        {
            var washTypes = await WashService.GetAllWashTypes();
            if (washTypes.exito)
            {
                var updateWashTypes = await this.AddNewWashTypes(washTypes);
                if (updateWashTypes)
                {
                    return washTypes;
                }
                else
                {
                    return await this.GetAllWashTypes();
                }
            }
            else
            {
                return await App.Database.GetAllWashTypes();
            }
        }

        private async Task<bool> AddNewWashTypes(MultivalueWash washTypes)
        {
            try
            {

                foreach (WashType washType in washTypes.lavados)
                {
                    var check = await GetWashType(washType.nombre_lavado);
                    if (check != null && check.nombre_lavado == null)
                    {
                        _database.Execute(@"INSERT INTO LAVADO
                         VALUES 
                         (? , ? , ? , ? , ? , ? )",
                        washType.nombre_lavado, washType.costo_personal, washType.precio,
                        washType.duracion, washType.puntos_otorgados, washType.costo_puntos);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        private async Task<WashType> GetWashType(string nombre_lavado)
        {
            WashType washType = new WashType();
            try
            {
                var washQuery = _database.Query<WashType>(@"SELECT LAVADO.NOMBRE_LAVADO, LAVADO.COSTO_PERSONAL, LAVADO.PRECIO,
                LAVADO.DURACION, LAVADO.PUNTOS_OTORGADOS, LAVADO.COSTO_PUNTOS
                FROM LAVADO WHERE LAVADO.NOMBRE_LAVADO = ?", nombre_lavado);

                if (washQuery.Count != 0)
                {
                    washType = washQuery[0];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return washType;
        }

        public async Task<MultivalueWash> GetAllWashTypes()
        {
            MultivalueWash washTypes = new MultivalueWash();
            try
            {

                var washQuery = _database.Query<WashType>(@"SELECT LAVADO.NOMBRE_LAVADO, LAVADO.COSTO_PERSONAL, LAVADO.PRECIO,
                LAVADO.DURACION, LAVADO.PUNTOS_OTORGADOS, LAVADO.COSTO_PUNTOS
                FROM LAVADO");
                washTypes.lavados = washQuery;
                washTypes.exito = true;
                return washTypes;
            }
            catch (Exception e)
            {
                washTypes.exito = false;
                return washTypes;
            }
        }

        public async Task<bool> AddAppointments(MultivalueAppointment appointments)
        {
            try
            {

                foreach (Appointment appointment in appointments.citas)
                {
                    var check = await GetAppointment(appointment.id_cita);
                    if (check != null && check.cedula_cliente == null)
                    {
                        _database.Execute(@"INSERT INTO CITA
                         VALUES 
                         (? , ? , ? , ? , ? , ? , ? , ?, ?, ?, ?, ?, ?)",
                        appointment.id_cita, appointment.cedula_cliente,
                        appointment.nombre_cliente,
                        appointment.apellido_cliente, appointment.placa_vehiculo,
                        appointment.nombre_sucursal, appointment.nombre_lavado,
                        appointment.cedula_trabajador, appointment.nombre_trabajador,
                        appointment.apellido_trabajador, appointment.hora,
                        appointment.facturada, appointment.duracion);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> AddPendingApp(NewAppRequest appointment)
        {
            try
            {
                _database.Execute(@"INSERT INTO CITA_PENDIENTE
                         VALUES 
                         (? , ? , ? , ? , ? , ? )",
                        appointment.cedula_cliente, appointment.placa_vehiculo,
                        appointment.nombre_sucursal,
                        appointment.nombre_lavado, appointment.hora,
                        appointment.facturada);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        private async Task<Appointment> GetAppointment(int id_cita)
        {
            Appointment appointment = new Appointment();
            try
            {
                var appointmentQuery = _database.Query<Appointment>(@"SELECT CITA.ID_CITA, CITA.CEDULA_CLIENTE,
                CITA.APELLIDO_CLIENTE, CITA.PLACA_VEHICULO, CITA.NOMBRE_SUCURSAL, CITA.NOMBRE_LAVADO,
                CITA.CEDULA_TRABAJADOR, CITA.NOMBRE_TRABAJADOR, CITA.APELLIDO_TRABAJADOR, CITA.HORA,
                CITA.FACTURADA, CITA.DURACION
                FROM CITA WHERE CITA.ID_CITA = ?", id_cita);

                if (appointmentQuery.Count != 0)
                {
                    appointment = appointmentQuery[0];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return appointment;
        }

        public async Task<MultivalueAppointment> GetAllAppointments()
        {
            MultivalueAppointment appointments = new MultivalueAppointment();
            try
            {

                var appointmentQuery = _database.Query<Appointment>(@"SELECT CITA.ID_CITA, CITA.CEDULA_CLIENTE, CITA.PLACA_VEHICULO,
                CITA.NOMBRE_SUCURSAL, CITA.NOMBRE_LAVADO, CITA.CEDULA_TRABAJADOR, CITA.HORA,
                CITA.FACTURADA, CITA.DURACION
                FROM CITA");
                appointments.citas = appointmentQuery;
                appointments.exito = true;
                return appointments;
            }
            catch (Exception e)
            {
                appointments.exito = false;
                return appointments;
            }
        }

        public async Task<List<NewAppRequest>> GetAllAppRequest()
        {
            List<NewAppRequest> appointments = new List<NewAppRequest>();
            try
            {

                var appointmentQuery = _database.Query<NewAppRequest>(@"SELECT CITA_PENDIENTE.NOMBRE_LAVADO, CITA_PENDIENTE.PLACA_VEHICULO,
                CITA_PENDIENTE.NOMBRE_SUCURSAL, CITA_PENDIENTE.HORA
                FROM CITA_PENDIENTE WHERE CITA_PENDIENTE.CEDULA_CLIENTE = ?",App.loggedUser.cedula_cliente);

                appointments = appointmentQuery;
                return appointments;
            }
            catch (Exception e)
            {
                return appointments;
            }
        }
        #endregion

        #region Creation
        public void CreateCustomerTable()
        {
            var table = _database.Execute(@"CREATE TABLE IF NOT EXISTS CLIENTE 
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
            var table = _database.Execute(@"CREATE TABLE IF NOT EXISTS CLIENTE_DIRECCION 
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
            var table = _database.Execute(@"CREATE TABLE IF NOT EXISTS CLIENTE_TELEFONO 
                (
                CEDULA_CLIENTE VARCHAR(9) NOT NULL ,
                TELEFONO VARCHAR(20) NOT NULL,
                FOREIGN KEY (CEDULA_CLIENTE) REFERENCES CLIENTE(CEDULA_CLIENTE)
                );");
            AddUserPhones();
        }

        public void CreateAppointmentTable()
        {
            var table = _database.Execute(@"CREATE TABLE IF NOT EXISTS CITA
                (
                ID_CITA INT NOT NULL,
                CEDULA_CLIENTE VARCHAR(9) NOT NULL,
                NOMBRE_CLIENTE VARCHAR(50) NOT NULL,
                APELLIDO_CLIENTE VARCHAR(50) NOT NULL,
                PLACA_VEHICULO VARCHAR(6) NOT NULL,
                NOMBRE_SUCURSAL VARCHAR(20) NOT NULL,
                NOMBRE_LAVADO VARCHAR(50) NOT NULL,
                CEDULA_TRABAJADOR VARCHAR(9) NOT NULL,
                NOMBRE_TRABAJADOR VARCHAR(50) NOT NULL,
                APELLIDO_TRABAJADOR VARCHAR(50) NOT NULL,
                HORA DATETIME NOT NULL,
                FACTURADA INT NOT NULL,
                DURACION INT NOT NULL,
                PRIMARY KEY (ID_CITA)
                );");
        }

        public void CreatePendingAppTable()
        {
            var table = _database.Execute(@"CREATE TABLE IF NOT EXISTS CITA_PENDIENTE
                (
                CEDULA_CLIENTE VARCHAR(9) NOT NULL,
                PLACA_VEHICULO VARCHAR(6) NOT NULL,
                NOMBRE_SUCURSAL VARCHAR(50) NOT NULL,
                NOMBRE_LAVADO VARCHAR(50) NOT NULL,
                HORA DATETIME NOT NULL,
                FACTURADA BIT NOT NULL
                );");
        }

        public void CreateBillTable()
        {
            var table = _database.Execute(@"CREATE TABLE IF NOT EXISTS FACTURA
            (
                ID_FACTURA INT NOT NULL,
                CEDULA_CLIENTE VARCHAR(9) NOT NULL,
                ID_CITA INT NOT NULL,
                CANTIDAD_SNACKS INT NOT NULL,
                CANTIDAD_BEBIDAS INT NOT NULL,
                PAGO_PUNTOS INT NOT NULL,
                PRIMARY KEY (ID_FACTURA)
                );");
        }

        public void CreateWashTable()
        {
            var table = _database.Execute(@"CREATE TABLE IF NOT EXISTS LAVADO
                (
                NOMBRE_LAVADO VARCHAR(50) NOT NULL,
                COSTO_PERSONAL INT NOT NULL,
                PRECIO INT NOT NULL,
                DURACION INT NOT NULL,
                PUNTOS_OTORGADOS INT NOT NULL,
                COSTO_PUNTOS INT NOT NULL,
                PRIMARY KEY (NOMBRE_LAVADO)
                );");
            AddWashTypes();
        }

        public void CreateOfficeTable()
        {
            var table = _database.Execute(@"CREATE TABLE IF NOT EXISTS SUCURSAL
                (
                NOMBRE_SUCURSAL VARCHAR(20) NOT NULL PRIMARY KEY ,
                TELEFONO VARCHAR(20) NOT NULL ,
                CEDULA_TRABAJADOR_GERENTE VARCHAR(9) NOT NULL,
                PROVINCIA VARCHAR(20) NOT NULL ,
                CANTON VARCHAR(20) NOT NULL ,
                DISTRITO VARCHAR(20) NOT NULL,
                FECHA_APERTURA DATE NOT NULL ,
                FECHA_INICIO_GERENCIA DATE NOT NULL
                );");
            AddLocations();
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
            catch (Exception e)
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
        private void AddWashTypes()
        {
            try
            {
                _database.Execute(@"INSERT INTO LAVADO
                VALUES
                ('Lavado y aspirado',2000,5000,30,5,20),
                ('Lavado encerado',3000,6000,60,10,30),
                ('Lavado premium y pulido',5000,8000,60,5,20);");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private void AddLocations()
        {
            try
            {
                _database.Execute(@"INSERT INTO SUCURSAL
                VALUES
                ('Montes de Oca', '25446675', '303450213', 'San José', 'Montes De Oca', 'San Pedro', '2019-01-01','2020-01-01'),
                ('San Carlos', '24438899', '222847727', 'Alajuela', 'San Carlos', 'Quesada', '2018-01-01','2021-01-01'),
                ('Alajuela', '25128341', '123121467', 'Alajuela', 'Cantón Central', 'Alajuela', '2017-01-01','2022-01-01');");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private void AddAppointments()
        {
            try
            {
                _database.Execute(@"INSERT INTO SUCURSAL
                VALUES
                ('Montes de Oca', '25446675', '303450213', 'San José', 'Montes De Oca', 'San Pedro', '2019-01-01','2020-01-01'),
                ('San Carlos', '24438899', '222847727', 'Alajuela', 'San Carlos', 'Quesada', '2018-01-01','2021-01-01'),
                ('Alajuela', '25128341', '123121467', 'Alajuela', 'Cantón Central', 'Alajuela', '2017-01-01','2022-01-01');");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
        #endregion




    }
}
