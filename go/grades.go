package main
import (
	"net/http"
	"fmt"
	"io"
	"io/ioutil"
	)

func main() {
	var r io.Reader
	res, err := http.Post("http://techmvs.technion.ac.il/cics/wmn/wmngrad?ORD=1",
		"function=signon&userid=36979821&password=50300072",
		r) // http://www.undergraduate.technion.ac.il/Tadpis.html")
	if err != nil {
		fmt.Printf("err %e", err)
		return
	}
	txt, err := ioutil.ReadAll(res.Body)
	if err != nil {
		fmt.Printf("err %e", err)
		return
	}
	res.Body.Close()
	fmt.Printf("%s", txt)
}