#ifndef __MAIN_H__
#define __MAIN_H__

struct Model
{
    const char *content;
    int extension_id;
    float fitness;
};

struct ModelCollection
{
    Model *models;
    int size;
};

extern "C" int GetSqliteVersion();
extern "C" int Connect(const char *path);
extern "C" int Disconnect();
extern "C" int GetExtensionId(const char *);
extern "C" int ResetTables();
extern "C" int AddModel(const char *, const char *, float);
extern "C" Model GetModel(int);
extern "C" ModelCollection GetBestModels(int);
extern "C" void DeleteCollection(ModelCollection &);
extern "C" void DeleteModel(Model &);

#endif // __MAIN_H__