#!/bin/bash

set -e

echo "### ASSEMBLING ###"
nasm --info -f win64 -o obj/puzzle.obj puzzle.asm

echo
echo "### LINKING ###"
golink //nw //dll //fo bin/puzzle.dll obj/puzzle.obj

echo
if [[ $2 == "rel" ]] || [[ $2 == "test-rel" ]]; then
  echo "### BUILDING RELEASE VERSION ###"
  build=Release
else
echo "### BUILDING DEBUG VERSION ###"
  build=Debug
fi
dotnet build -c $build

echo
if [[ $2 == "test" ]] || [[ $2 == "test-rel" ]]; then
  echo "### TEST MODE ###"
  input="input-test$3.txt"
else
  input=input.txt
fi
time "./bin/$build/net10.0/Aoc" $1 "$input"
