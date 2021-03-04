#include <iostream>
#include "main.h"
#include "sqlite3/sqlite3.h"

using namespace std;

sqlite3 *handle;

extern "C" int getSqliteVersion()
{
    return sqlite3_libversion_number();
}

extern "C" int connect(const char *path)
{
    return sqlite3_open(path, &handle);
}

extern "C" int disconnect()
{
    return sqlite3_close(handle);
}

extern "C" int getExtensionId(const char *name)
{
    sqlite3_stmt *statement;
    if (sqlite3_prepare(handle, "SELECT id FROM extensions WHERE name=?;", -1, &statement, NULL) == SQLITE_OK)
    {
        if (sqlite3_bind_text(statement, 1, name, -1, NULL) == SQLITE_OK)
        {
            if (sqlite3_step(statement) == SQLITE_ROW)
            {
                int extension_id = sqlite3_column_int(statement, 0);
                if (sqlite3_finalize(statement) == SQLITE_OK)
                    return extension_id;
            }
        }
    }

    return 0;
}