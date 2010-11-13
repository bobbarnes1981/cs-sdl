#!/bin/sh
cd `dirname "$0"`/..
php $*
echo
echo
read -p "Press any key to continue..."
