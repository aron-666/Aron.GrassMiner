#!/bin/sh
/vpnclient/vpnclient start
/usr/bin/dotnet Aron.GrassMiner.dll
exit $?