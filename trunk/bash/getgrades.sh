#!/bin/bash
curl -s -L -d\ {function=signon,userid=$1,password=$2} $(curl -s -L http://techmvs.technion.ac.il/cics/wmn/wmngrad?ORD=1 | egrep -o http.*s=1)
