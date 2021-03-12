#ifndef __MAIN_H__
#define __MAIN_H__

#include <string>

struct Model
{
    const char *content;
    int extension_id;
    float fitness;
};

struct ModelCollection
{
    Model **models;
    int size;
};

extern "C"
{
    int GetSqliteVersion();
    int Connect(const char *path);
    int ConnectAndSetup(const char *path);
    int Disconnect();
    int GetExtensionId(const char *);
    int ResetTables();
    int AddModel(const char *, int, float);
    Model *GetModel(int);
    ModelCollection *GetBestModels(int);
    void DeleteCollection(ModelCollection *);
    void DeleteModel(Model *);
}

#endif // __MAIN_H__