#include "main.h"
#include "sqlite3/sqlite3.h"
#include <iostream>

using namespace std;

sqlite3 *handle;

extern "C" int GetSqliteVersion()
{
    return sqlite3_libversion_number();
}

extern "C" int Connect(const char *path)
{
    return sqlite3_open(path, &handle);
}

extern "C" int Disconnect()
{
    return sqlite3_close(handle);
}

extern "C" int GetExtensionId(const char *name)
{
    sqlite3_stmt *statement;
    if (sqlite3_prepare(handle, "SELECT id FROM extensions WHERE name=?", -1, &statement, NULL) == SQLITE_OK)
        if (sqlite3_bind_text(statement, 1, name, -1, NULL) == SQLITE_OK)
            if (sqlite3_step(statement) == SQLITE_ROW)
            {
                int extension_id = sqlite3_column_int(statement, 0);
                if (sqlite3_finalize(statement) == SQLITE_OK)
                    return extension_id;
            }

    return 0;
}

extern "C" int Reset()
{
    return sqlite3_exec(handle, "DROP TABLE IF EXISTS models; DROP TABLE IF EXISTS extensions; CREATE TABLE models (id INTEGER PRIMARY KEY AUTOINCREMENT, content TEXT NOT NULL, extension_id INT NOT NULL, fitness FLOAT(12) NOT NULL, FOREIGN KEY (extension_id) REFERENCES extensions(id)); CREATE TABLE extensions (id INTEGER PRIMARY KEY AUTOINCREMENT, name VARCHAR(255) NOT NULL UNIQUE); INSERT INTO extensions (name) VALUES('unity-neat'); INSERT INTO extensions (name) VALUES('neat-pack')", NULL, NULL, NULL);
}

extern "C" int AddModel(const char *content, const char *extension_name, float fitness)
{
    int extension_id = GetExtensionId(extension_name);

    if (extension_id != 0)
    {
        sqlite3_stmt *statement;
        if (sqlite3_prepare(handle, "INSERT INTO models (content, extension_id, fitness) VALUES(?, ?, ?);", -1, &statement, NULL) == SQLITE_OK)
        {
            int bind_rc = sqlite3_bind_text(statement, 1, content, -1, NULL);
            int bind_rc2 = sqlite3_bind_int(statement, 2, extension_id);
            int bind_rc3 = sqlite3_bind_double(statement, 3, fitness);

            if (bind_rc == SQLITE_OK && bind_rc2 == SQLITE_OK && bind_rc3 == SQLITE_OK && sqlite3_step(statement) == SQLITE_DONE)
                return sqlite3_finalize(statement);
        }
    }

    return SQLITE_ERROR;
}
