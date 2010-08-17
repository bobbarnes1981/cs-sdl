#!/bin/sh
cd `dirname "$0"`/..
nice -n 10 php $*
echo
echo
read -p "Press any key to continue..."
