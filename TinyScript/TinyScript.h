#include <iostream>
#include <string>
#include <vector>
#include <cstdarg>
#include <cstdlib>

int Min(int num, ...)
{
    va_list arglist;
    va_start(arglist, num);
    int result = va_arg(arglist, int);
    for (int i = 1; i < num; i++)
    {
        result = min(va_arg(arglist, int), result);
    }
    va_end(arglist);
    return result;
}

int Max(int num, ...)
{
    va_list arglist;
    va_start(arglist, num);
    int result = va_arg(arglist, int);
    for (int i = 1; i < num; i++)
    {
        result = min(va_arg(arglist, int), result);
    }
    va_end(arglist);
    return result;
}