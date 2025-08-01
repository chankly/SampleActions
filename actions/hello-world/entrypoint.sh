#!/bin/sh -l

echo "Hello $1"
currentTime=$(date)
echo "time=$currentTime" >> $GITHUB_OUTPUT