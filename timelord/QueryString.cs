﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace timelord
{
    public static class QueryString
    {
        public static class Task
        {
            public const string Create = "CREATE TABLE IF NOT EXISTS task (id INTEGER PRIMARY KEY AUTOINCREMENT, description TEXT, begindate TEXT, enddate TEXT, status TEXT)";
            public const string Select = "SELECT id,description,begindate,enddate,status FROM task";
        }
        public static class Client
        {
            public const string Create = "CREATE TABLE IF NOT EXISTS client ( id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, address TEXT, phone TEXT, email TEXT, date_created TEXT, hourly_rate REAL)";
            public const string Select = "SELECT id,name,address,phone,email,date_created,hourly_rate FROM Client";
        }

        public static class Identity
        {
            public const string Create = "CREATE TABLE IF NOT EXISTS identity ( id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, address TEXT, phone TEXT, email TEXT, date_created TEXT, hourly_rate REAL)";
            public const string Select = "SELECT id,name,address,phone,email,date_created,hourly_rate FROM Identity";
        }

        public static class Invoice
        {
            public const string Create = "CREATE TABLE IF NOT EXISTS invoice (id INTEGER PRIMARY KEY AUTOINCREMENT,date_created	TEXT NOT NULL,date_due TEXT,id_client INTEGER NOT NULL, id_identity INTEGER NOT NULL)";
            public const string Select = "SELECT id,date_created,due_date,id_client,id_identity FROM Invoice";
        }

        public static class InvoiceTask
        {
            public const string Create = "CREATE TABLE IF NOT EXISTS invoice_task ( id INTEGER PRIMARY KEY AUTOINCREMENT, id_invoice INTEGER NOT NULL, id_task INTEGER NOT NULL)";
            public const string Select = "SELECT id,id_invoice,id_task FROM InvoiceTask";
        }
    }
}
