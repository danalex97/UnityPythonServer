import socket
import re
import sys
from subprocess import call

port = 0
if len(sys.argv) == 1:
	port = 8000
else:
	port = int(sys.argv[1])
print "Server starting on port " + str(port)

################################################################################

host = ''
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

# allows reuse of the socket once the process gets stopped
sock.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)

#bind the connection
sock.bind((host, port))

#listen to one client
sock.listen(1)

################################################################################

import signal
import sys

#register a specific signal handler if necessary at the termination of the program
# def signal_handler(signal, frame):
#     # close the socket here
#     sys.exit(0)
# signal.signal(signal.SIGINT, signal_handler)

################################################################################

while True:
    csock, caddr = sock.accept()
    print "Connection from:" + `caddr`

    # receive the GET from front-ent
    req = csock.recv(1024)

    # # process the received input
    lines = req.split('\n')

    # the default client response will be the server that we will use to test
    # TODO: use Python client & then use Unity client
    csock.sendall("""HTTP/1.0 200 OK
		Content-Type: text/html
		<html>
			<head>
				<title>Success</title>
			</head>
			<body>
				Success
			</body>
		</html>
	""" )
