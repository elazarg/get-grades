#!/usr/bin/python3

import sys
from html.parser import HTMLParser

if ('--help' in sys.argv):
	print('Usage: python grades.py [<userid> <password>] [<outfilename.csv>] [-q] [--help]')
	sys.exit()

if ('-q' in sys.argv):
	def print(*x, sep=None, end=None):
		pass
	def input(*x):
		pass



def trans(w):
	if [i for i in w if 1488 <= ord(i) <= 1514] == []:
		return w
	return ''.join(reversed(w)).translate({40: 41, 41 : 40})

table = []

class TableToCSV(HTMLParser):
	def handle_starttag(self, tag, attrs):
		if tag in ['TD', 'td']:
			table[-1].append('')
		elif tag in ['TR', 'tr']:
			table.append([])

	def handle_data(self, data):
		#fancy output
		print('\r', self.getpos()[0] * 100 // TableToCSV.lines, sep = '', end ='%')
		#arbitrary delimiters:
		if data.isdigit() and len(data)==6 or '%' in data:
			table[-1].append('') 
		try:
			if data not in ['\n', ' ']:
				table[-1][-1] += data.strip() + ' '
		except:
			pass
def validate_username(x):
	return 8 <= len(x) <= 9 and x.isdigit()

def validate_password(x):
	return len(x) == 8 and x.isdigit()

def get_details():
	def get_and_validate(name, validate):
		x = input(name + ': ')
		if validate(x):
			return x
		print('invalid ' + name)
		return get_and_validate(name, validate)
	userid = get_and_validate('username', validate_username)
	password = get_and_validate('password', validate_password)
	return userid, password

def get_from_web(userid, password):
	from urllib.request import urlopen
	BASEURL = 'http://techmvs.technion.ac.il:80/cics/wmn/wmngrad?'
	logstr = bytes('function=signon&userid='+userid+'&password='+password,'ascii')
	code = ''
	for i in range(5):
		try:
			u = urlopen(BASEURL + code + 'ORD=1', logstr, 5)
			data = u.read().decode("hebrew")
			if 'Handle_Empty' not in data:
				TableToCSV.lines = data.count('\n')
				return data
			print('\rconnection attempts:', i + 1, end ='')
			code = data.partition(BASEURL)[2][:8]
		except:	
			print()
			input('connection error. press any key to try again. ')
	print()
	print('make sure your username and password are correct')
	return get_from_web(*get_details())

def cook(table):
	for i, line in enumerate(table):
		table[i] = ' '.join( (trans(i) for i in reversed(' , '.join(line).split()) ) )

def write_output(filename, table):
	data = '\n'.join(table[1:])
	open(filename, mode = 'wb').write(bytes(data, encoding='utf-16'))

def get_from_file(inputfile):
	return ''.join(open(inputfile , encoding = 'hebrew'))

def parse_cmd_line():
	files = [j for j in sys.argv if j[-4:]=='.csv']
	filename = files[0] if len(files) > 0 else 'grades.csv'

	nums = [j for j in sys.argv if j.isdigit()]
	if len(nums) >= 2 and validate_username(nums[0]) and validate_password(nums[1]):
		userid, password = nums[0:2]
	else:
		userid, password = get_details()

	return filename, userid, password

def main():
	filename, userid, password = parse_cmd_line()
	print('Trying to connect. userid=', userid, ', password=', password, sep = '')
	data = get_from_web(userid, password)
	print()
	print('Reading HTML file:')
	TableToCSV().feed(data)
	print()
	print('Processing HTML file.')
	cook(table)
	print('Writing output: ', end = '')
	write_output(filename, table)
	print(filename)

if __name__=='__main__':
	main()

