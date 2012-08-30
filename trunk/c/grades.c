#include <curl/curl.h>
#include <stdlib.h>
#include <stdio.h>
#include <string.h>

const char* userid;
const char* password;

void validate_authentication_details(void)
{
	int lenuser = strlen(userid);
	int lenpassword = strlen(password);
	int i, j;
	for (i = 0; i < 10 && isdigit(userid[i]); i++);
	for (j = 0; j < 9 && isdigit(password[j]); j++);
	if (userid[i] != '\0' || password[j] != '\0') {
		printf("bad username/password\n");
		exit(1);
	}
}

const char* get_redirect(FILE* page)
{
	static double code = 0;
	rewind(page);
	fseek ( page , 3667, SEEK_SET );
	if (fgetc(page) == '?') {
		fread(&code, 8, 1, page);
		printf("%.8s\n", (const char*)&code);
		return (const char*)&code;
	} 

	return NULL;
}

void set_address(CURL *hnd, const char* code)
{
	const char* addressfmt = "http://techmvs.technion.ac.il/cics/wmn/wmngrad?%.8s&ORD=1";
	char address[128];
	sprintf(address, addressfmt, code);
	curl_easy_setopt(hnd, CURLOPT_URL, address);
}

void set_postfields(CURL *hnd, const char* code)
{
	if (code[0] == 0)
		return;

	const char* postfieldsfmt = strlen(userid) == 8
			  ? "function=signon&userid=%.8s&password=%.8s"
			  : "function=signon&userid=%.9s&password=%.8s";
	static char postfields[64];
	sprintf(postfields, postfieldsfmt, userid, password);

	curl_easy_setopt(hnd, CURLOPT_POSTFIELDS, postfields);
	curl_easy_setopt(hnd, CURLOPT_POSTFIELDSIZE_LARGE, (curl_off_t)strlen(postfields));
}

int send_request(FILE * file, const char* code)
{
	CURL *hnd = curl_easy_init();
	curl_easy_setopt(hnd, CURLOPT_FOLLOWLOCATION, 1L);
	curl_easy_setopt(hnd, CURLOPT_WRITEDATA, file);
	set_address(hnd, code);
	set_postfields(hnd, code);

	CURLcode ret = curl_easy_perform(hnd);
	if (ret != 0) {
		printf("%s\n", curl_easy_strerror(ret));
		return (int)ret;
	}
	curl_easy_cleanup(hnd);

	const char* redirection_code = get_redirect(file);
	fclose(file);

	if (redirection_code == NULL)
		return (int)ret;

	return send_request(fopen("cgrades.html", "w"), redirection_code);
}

int main(int argc, char *argv[])
{
	if (argc < 3) {
		printf("usage\n");
		exit(1);
	}
	userid = argv[1];
	password = argv[2];
	validate_authentication_details();
	send_request(tmpfile(), "");

	printf("done\n");
}
