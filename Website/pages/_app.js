// The order of css imports determines the order of css rules
// The last imported css file has the highest priority
import "bootstrap/dist/css/bootstrap.min.css";
import "../styles/global.css";

// Layout component defining global components
import Layout from "../components/_layout.jsx";
import Script from "next/script";

function MyApp({ Component, pageProps }) {
  return (
    // Inside the layout is the corresponding page
    // Layout applies any components that are common to all pages
    <>
      <Script
        src="https://code.jquery.com/jquery-3.5.1.slim.min.js"
        crossorigin="anonymous"
      ></Script>
      <Layout>
        <Component {...pageProps} />
      </Layout>
    </>
  );
}

export default MyApp;
