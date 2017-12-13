#include <iostream>
#include <string>
#include <vector>
#include <cstdarg>
#include <cstdlib>
#include <sstream>
#include <algorithm>

int min(int num, ...)
{
    va_list arglist;
    va_start(arglist, num);
    int result = va_arg(arglist, int);
    for (int i = 1; i < num; i++)
    {
        result = std::min(va_arg(arglist, int), result);
    }
    va_end(arglist);
    return result;
}

int max(int num, ...)
{
    va_list arglist;
    va_start(arglist, num);
    int result = va_arg(arglist, int);
    for (int i = 1; i < num; i++)
    {
        result = std::max(va_arg(arglist, int), result);
    }
    va_end(arglist);
    return result;
}

std::string tostring(int i)
{
    std::stringstream ss;
    ss << i;
    return ss.str();
}

std::string tostring(const char *str)
{
    return str;
}

std::string tostring(const std::string &str)
{
    return str;
}

std::string tostring(bool b)
{
    return b ? "true" : "false";
}