
rem MIT License

rem Copyright (c) 2020 huailiang


mkdir build64 & pushd build64
cmake -G "Visual Studio 15 2017 Win64" ..
popd
cmake --build build64 --config Release
md Plugins\x86_64
copy /Y build64\Release\Entitas.dll Plugins\x86_64\Entitas.dll
pause