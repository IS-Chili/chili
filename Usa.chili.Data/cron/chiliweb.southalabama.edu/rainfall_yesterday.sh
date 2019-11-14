#!/bin/bash

dateName=$( date +%Y-%m-%d -d "yesterday" )

./testDBAccess.py $dateName
