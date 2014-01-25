begin
  require 'bundler/setup'
  require 'fuburake'
rescue LoadError
  puts 'Bundler and all the gems need to be installed prior to running this rake script. Installing...'
  system("gem install bundler --source http://rubygems.org")
  sh 'bundle install'
  system("bundle exec rake", *ARGV)
  exit 0
end


@solution = FubuRake::Solution.new do |sln|
  sln.compile = {
    :solutionfile => 'src/FubuMVC.ServerSentEvents.sln'
  }

  sln.assembly_info = {
    :product_name => "FubuMVC.ServerSentEvents",
    :copyright => "Copyright 2012-#{Time.now.year} Jeremy D. Miller, Matt Smith, et al. All rights reserved."
  }

  sln.ripple_enabled = true
  sln.fubudocs_enabled = true

  sln.assembly_bottle 'FubuMVC.ServerSentEvents'
end

storytellerProject = "src/FubuMVC.ServerSentEvents.Storyteller"

FubuRake::Storyteller.new({
  :path => storytellerProject,
  :compilemode => @solution.compilemode
})

FubuRake::Storyteller.new({
  :path => storytellerProject,
  :prefix => 'st:phantom',
  :profile => 'phantom',
  :compilemode => @solution.compilemode
})

FubuRake::Storyteller.new({
  :path => storytellerProject,
  :prefix => 'st:chrome',
  :profile => 'chrome',
  :compilemode => @solution.compilemode
})
