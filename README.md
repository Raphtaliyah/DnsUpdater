# DnsUpdater
An open source cross platform dynamic dns record updater for [NameSilo](https://www.namesilo.com/).

This was developed for API version 1, if this version is not supported anymore, please look for a more recent solution.

## Configuration file
Running it once will get you the default configuration file, or you could create a `Config.json` file in the working directory.
```
{
  "ApiKey": "NAMESILO API KEY HERE",
  "IntervalSeconds": 300,
  "Domains": {
    "example.com": [
      ""
    ]
  }
}
```
You can get your api key [here](https://www.namesilo.com/account/api-manager)! (Make sure you don't enable any ip restriction!)

By default an update cycle runs every 5 minutes, you can change it by editing `IntervalSeconds`.

Domains are stored in groups as a dictionary where the key is the primary domain and the value is an array of the subdomains, the naked domain is an empty string (null also works but there is no guarantee that it will stay like this!) Let's look at an example:

`subdomain.example.com` will be represented like this:
```
"example.com": [
  "subdomain"
]
```
and if you also wish to include the primary domain:
```
"example.com": [
  "",
  "subdomain"
]
```
You can also have more domains:
```
"example.com": [
  "",
  "subdomain"
],
"example.org": [
  ""
]
```
make sure you don't include `http`, `https` or any `/` and it should work!

---

After you have configured everything, use your preferred method for auto starting applications to run this at startup.
